using Gpt4All.Bindings;

namespace Gpt4All.Chat;

public interface IChatConversation
{
    Guid ConversationId { get; }

    /// <summary>
    /// The context of this chat
    /// </summary>
    LLModelPromptContext Context { get; set; }

    /// <summary>
    /// The messages in this conversation
    /// </summary>
    ICollection<ChatMessage> Messages { get; set; }

    /// <summary>
    /// Add a message to this conversation
    /// </summary>
    /// <param name="authorRole">Role of the message author</param>
    /// <param name="content">Message content</param>
    void AddMessage(ChatRole authorRole, string content);
}
