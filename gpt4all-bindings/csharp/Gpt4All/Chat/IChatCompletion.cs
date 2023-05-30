namespace Gpt4All.Chat;

public interface IChatCompletion
{
    IChatConversation CreateNewChat();

    /// <summary>
    /// Generate a new chat message
    /// </summary>
    /// <param name="chat">The chat</param>
    /// <param name="requestOptions">The completion settings</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
    /// <returns>The generated message</returns>
    Task<ITextPredictionResult> GetMessageAsync(
        IChatConversation chat,
        PredictRequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate a new chat message
    /// </summary>
    /// <param name="chat">The chat</param>
    /// <param name="requestOptions">The completion settings</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
    /// <returns>The generated message</returns>
    Task<ITextPredictionStreamingResult> GetStreamingMessageAsync(
        IChatConversation chat,
        PredictRequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default);
}
