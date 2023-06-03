using Gpt4All.Chat;

namespace Gpt4All;

public static class IGpt4AllModelExtensions
{
    public static Task<ITextPredictionStreamingResult> RegenerateChatResponse(
        this IGpt4AllModel model, IChatConversation chat, int pastConversationTokens, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chat);

        if (chat.Messages.Count == 0)
        {
            throw new InvalidOperationException("The chat must contain atleast one prompt/response message couple.");
        }

        if (pastConversationTokens <= 0)
        {
            throw new ArgumentException($"{nameof(pastConversationTokens)} must be > 0");
        }

        // delete the last assistant message
        chat.Messages = chat.Messages.Take(chat.Messages.Count - 1).ToList();

        // adjust the number of tokens in past conversation
        chat.Context.PastNum -= Math.Max(0, chat.Context.PastNum - pastConversationTokens);

        // regenerate the response
        return model.GetStreamingMessageAsync(chat, cancellationToken: cancellationToken);
    }
}
