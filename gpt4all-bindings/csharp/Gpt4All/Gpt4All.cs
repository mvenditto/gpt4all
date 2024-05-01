using System.Diagnostics;
using System.Runtime.CompilerServices;
using Gpt4All.Bindings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: InternalsVisibleTo("Gpt4All.Tests")]

namespace Gpt4All;

public class Gpt4All : Gpt4AllModelBase, IGpt4AllModel
{
    private readonly LLModelPromptContext _context;
    private readonly ILogger<Gpt4All> _logger;

    private const string ResponseErrorMessage =
        "The model reported an error during token generation error={ResponseError}";

    /// <inheritdoc/>
    public IPromptFormatter? PromptFormatter { get; set; }

    public LLModelPromptContext Context => _context;

    internal Gpt4All(ILLModel model, ILogger<Gpt4All>? logger = null) : base(model, logger)
    {
        _context = new LLModelPromptContext();
        _logger = logger ?? NullLogger<Gpt4All>.Instance;
    }

    /// <inheritdoc/>
    public Task<ITextPredictionResult> GetPredictionAsync(
        string prompt,
        PredictRequestOptions opts,
        string promptTemplate = "%1",
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(prompt);

        return Task.Run(() =>
        {
            _logger.LogInformation("Start prediction task");

            var sw = Stopwatch.StartNew();
            var result = new TextPredictionResult();

            try
            {
                _context.Update(opts);

                if (PromptFormatter != null)
                {
                    _logger.LogWarning("PromptFormatter is deprecated. Use a ChatSession with a PromptTemplate instead.");
                }

                var promptTemplate_ = PromptFormatter != null ? "%1" : promptTemplate;
                var prompt_ = PromptFormatter != null ? PromptFormatter.FormatPrompt(prompt) : prompt;

                _model.Prompt(prompt_, promptTemplate_, _context, responseCallback: e =>
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
                }, special: opts.Special, cancellationToken: cancellationToken);
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

    /// <inheritdoc/>
    public Task<ITextPredictionStreamingResult> GetStreamingPredictionAsync(
        string prompt,
        PredictRequestOptions opts,
        string promptTemplate = "%1",
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(prompt);

        var result = new TextPredictionStreamingResult();

        _ = Task.Run(() =>
        {
            _logger.LogInformation("Start streaming prediction task");

            var sw = Stopwatch.StartNew();

            try
            {
                _context.Update(opts);

                if (PromptFormatter != null)
                {
                    _logger.LogWarning("PromptFormatter is deprecated. Use a ChatSession with a PromptTemplate instead.");
                }

                var promptTemplate_ = PromptFormatter != null ? "%1" : promptTemplate;
                var prompt_ = PromptFormatter != null ? PromptFormatter.FormatPrompt(prompt) : prompt;

                _model.Prompt(prompt_, promptTemplate_, _context, responseCallback: e =>
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
                }, special: opts.Special, cancellationToken: cancellationToken);
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
}
