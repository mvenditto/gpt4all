using System.Diagnostics;
using System.Text;
using Gpt4All.Bindings;
using Gpt4All.Chat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gpt4All;

public class Gpt4All : IGpt4AllModel
{
    private readonly ILLModel _model;
    private readonly ILogger _logger;
    private IPromptFormatter _promptFormatter;

    private const string ResponseErrorMessage =
        "The model reported an error during token generation error={ResponseError}";

    /// <inheritdoc/>
    public IPromptFormatter? PromptFormatter
    {
        get => _promptFormatter;
        set => _promptFormatter = value ?? new DefaultPromptFormatter();
    }

    internal Gpt4All(ILLModel model, ILogger? logger = null)
    {
        _model = model;
        _logger = logger ?? NullLogger.Instance;
        _promptFormatter = new DefaultPromptFormatter();
    }

    private string FormatPrompt(string prompt)
    {
        return _promptFormatter.FormatPrompt(prompt);
    }

    private string FormatChatPrompt(IChatConversation chat)
    {
        return _promptFormatter.FormatChatPrompt(chat);
    }

    public Task<ITextPredictionResult> GetPredictionAsync(string text, PredictRequestOptions opts, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(text);

        return Task.Run(() =>
        {
            _logger.LogInformation("Start prediction task");

            var sw = Stopwatch.StartNew();
            var result = new TextPredictionResult();
            var context = opts.ToPromptContext();
            var prompt = FormatPrompt(text);
            _logger.LogDebug("Final prompt: {FinalPrompt}", prompt);

            try
            {
                var usage = _model.Prompt(prompt, context, responseCallback: e =>
                {
                    if (e.IsError)
                    {
                        _logger.LogWarning(ResponseErrorMessage, e.Response);
                        result.Success = false;
                        result.ErrorMessage = e.Response;
                        return false;
                    }
                    result.Append(e.Response);
                    return true;
                }, cancellationToken: cancellationToken);

                result.Usage = usage.ToPredictionInfo();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Prompt error");
                result.Success = false;
            }

            sw.Stop();
            _logger.LogInformation("Prediction task completed elapsed={Elapsed}s", sw.Elapsed.TotalSeconds);

            return (ITextPredictionResult)result;
        }, CancellationToken.None);
    }

    public Task<ITextPredictionStreamingResult> GetStreamingPredictionAsync(string text, PredictRequestOptions opts, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(text);

        var result = new TextPredictionStreamingResult();

        _ = Task.Run(() =>
        {
            _logger.LogInformation("Start streaming prediction task");
            var sw = Stopwatch.StartNew();

            try
            {
                var context = opts.ToPromptContext();
                var prompt = FormatPrompt(text);
                _logger.LogDebug("Final prompt: {FinalPrompt}", prompt);

                var usage = _model.Prompt(prompt, context, responseCallback: e =>
                {
                    if (e.IsError)
                    {
                        _logger.LogWarning(ResponseErrorMessage, e.Response);
                        result.Success = false;
                        result.ErrorMessage = e.Response;
                        return false;
                    }
                    result.Append(e.Response);
                    return true;
                }, cancellationToken: cancellationToken);

                result.Usage = usage.ToPredictionInfo();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Prompt error");
                result.Success = false;
            }
            finally
            {
                result.Complete();
                sw.Stop();
                _logger.LogInformation("Prediction task completed elapsed={Elapsed}s", sw.Elapsed.TotalSeconds);
            }
        }, CancellationToken.None);

        return Task.FromResult((ITextPredictionStreamingResult)result);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _model.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IChatConversation CreateNewChat()
    {
        var chat = new ChatConversation();
        return chat;
    }

    public async Task<ITextPredictionResult> GetMessageAsync(IChatConversation chat, PredictRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chat);

        var opts = requestOptions ?? PredictRequestOptions.Chat;

        chat.Context.ApplyOptions(opts);

        var finalPrompt = FormatChatPrompt(chat);

        _logger.LogDebug("Final prompt: {FinalPrompt}", finalPrompt);

        var result = await Task.Run(() =>
        {
            var result = new TextPredictionResult();

            var usage = _model.Prompt(finalPrompt, chat.Context, responseCallback: e =>
            {
                if (e.IsError)
                {
                    result.Success = false;
                    result.ErrorMessage = e.Response;
                    return false;
                }
                result.Append(e.Response);
                return true;
            }, cancellationToken: cancellationToken);

            result.Usage = usage.ToPredictionInfo();

            return (ITextPredictionResult)result;
        }, CancellationToken.None);

        var assistantMessage = await result.GetPredictionAsync(cancellationToken);

        chat.AddMessage(ChatRole.Assistant, assistantMessage);

        return result;
    }

    public Task<ITextPredictionStreamingResult> GetStreamingMessageAsync(IChatConversation chat, PredictRequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chat);

        var opts = requestOptions ?? PredictRequestOptions.Chat;

        chat.Context.ApplyOptions(opts);

        var finalPrompt = FormatChatPrompt(chat);

        _logger.LogDebug("Final prompt: {FinalPrompt}", finalPrompt);

        var result = new TextPredictionStreamingResult();

        var assistantMessage = new StringBuilder();

        _ = Task.Run(() =>
        {
            try
            {
                var usage = _model.Prompt(finalPrompt, chat.Context, responseCallback: e =>
                {
                    if (e.IsError)
                    {
                        result.Success = false;
                        result.ErrorMessage = e.Response;
                        return false;
                    }
                    result.Append(e.Response);
                    assistantMessage.Append(e.Response);
                    return true;
                }, cancellationToken: cancellationToken);

                result.Usage = usage.ToPredictionInfo();
            }
            finally
            {
                result.Complete();
            }
        }, CancellationToken.None).ContinueWith(_ =>
        {
            if (result.Success)
            {
                chat.AddMessage(ChatRole.Assistant, assistantMessage.ToString());
            }
        }, cancellationToken);

        return Task.FromResult((ITextPredictionStreamingResult)result);
    }

    public void SetThreadCount(int threadCount)
    {
        _model.SetThreadCount(threadCount);
    }
}
