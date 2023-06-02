using Gpt4All.Bindings;

namespace Gpt4All.Chat;

public class ChatConversation : IChatConversation
{
    private int _incrementalMessageId;

    public Guid ConversationId { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public ICollection<ChatMessage> Messages { get; set; }

    /// <inheritdoc/>
    public LLModelPromptContext Context { get; set; }

    public ChatConversation()
    {
        Messages = new List<ChatMessage>();
        Context = PredictRequestOptions.Chat.ToPromptContext();
    }

    /// <inheritdoc/>
    public void AddMessage(ChatRole authorRole, string content)
    {
        Messages.Add(new ChatMessage(authorRole, content)
        {
            MessageId = _incrementalMessageId++
        });
    }
}
