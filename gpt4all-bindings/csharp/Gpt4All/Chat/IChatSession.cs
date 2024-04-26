namespace Gpt4All.Chat;

public interface IChatSession
{
    string PromptTemplate { get; init; }

    string? SystemPrompt { get; init; }

    IReadOnlyCollection<ChatMessage> Messages { get; }

    Task<string?> GetResponseAsync(string prompt, PredictRequestOptions opts, CancellationToken cancellationToken = default);

    IAsyncEnumerable<string> GetStreamingResponseAsync(string prompt, PredictRequestOptions opts, CancellationToken cancellationToken = default);
}
