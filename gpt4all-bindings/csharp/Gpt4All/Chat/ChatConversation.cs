namespace Gpt4All.Chat;

public class ChatConversation : IChatConversation
{
    private readonly List<ChatMessage> _messages;

    private int _incrementalMessageId;

    public Guid ConversationId { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public IEnumerable<ChatMessage> Messages => _messages;

    public ChatConversation()
    {
        _messages = new List<ChatMessage>();
    }

    /// <inheritdoc/>
    public void AddMessage(ChatRole authorRole, string content)
    {
        _messages.Add(new ChatMessage(authorRole, content)
        {
            MessageId = _incrementalMessageId++
        });
    }
}
