namespace Gpt4All.Chat;

public interface IChatConversation
{
    Guid ConversationId { get; }

    /// <summary>
    /// The messages in this conversation
    /// </summary>
    IEnumerable<ChatMessage> Messages { get; }

    /// <summary>
    /// Add a message to this conversation
    /// </summary>
    /// <param name="authorRole">Role of the message author</param>
    /// <param name="content">Message content</param>
    void AddMessage(ChatRole authorRole, string content);
}
