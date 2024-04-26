
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Gpt4All.Chat;

public partial class ChatSession : IChatSession
{
    private readonly ITextPrediction _model;

    private readonly List<ChatMessage> _messages;

    private static readonly Regex PromptTemplatePlaceholdersRegex = PromptTemplatePlaceholders();

    /// <inheritdoc/>
    public IReadOnlyCollection<ChatMessage> Messages => _messages;

    public string? SystemPrompt { get; init; }

    public string PromptTemplate { get; init; }

    public ChatSession(ITextPrediction model, string promptTemplate, string? systemPrompt = null)
    {
        _model = model;
        _messages = new List<ChatMessage>(capacity: 1);

        if (string.IsNullOrEmpty(promptTemplate))
        {
            throw new ArgumentException(
                "Prompt template cannot be null or empty",
                nameof(promptTemplate));
        }

        if (PromptTemplatePlaceholdersRegex.IsMatch(promptTemplate))
        {
            throw new ArgumentException(
                "Prompt template containing a literal '%1' is not supported. For a prompt placeholder, please use '{0}' instead.",
                nameof(promptTemplate));
        }

        PromptTemplate = promptTemplate;
        SystemPrompt = systemPrompt;
        _messages.Add(new ChatMessage(ChatRole.System, systemPrompt ?? string.Empty));
    }

    public async Task<string?> GetResponseAsync(string prompt, PredictRequestOptions opts, CancellationToken cancellationToken = default)
    {
        _messages.Add(new ChatMessage(ChatRole.User, prompt));

        var result = await _model.GetPredictionAsync(prompt, opts, cancellationToken);

        if (result.Success)
        {
            var content = await result.GetPredictionAsync(cancellationToken);
            _messages.Add(new ChatMessage(ChatRole.Assistant, content));
            return content;
        }

        return null;
    }

    public async IAsyncEnumerable<string> GetStreamingResponseAsync(string prompt, PredictRequestOptions opts, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _messages.Add(new ChatMessage(ChatRole.User, prompt));

        var result = await _model.GetStreamingPredictionAsync(prompt, opts, cancellationToken);

        var content = new StringBuilder();

        await foreach (var token in result.GetPredictionStreamingAsync(cancellationToken))
        {
            content.Append(token);
            yield return token;
        }

        if (result.Success)
        {
            _messages.Add(new ChatMessage(ChatRole.Assistant, content.ToString()));
        }
    }

    [GeneratedRegex("%1(?![0-9])")]
    private static partial Regex PromptTemplatePlaceholders();
}
