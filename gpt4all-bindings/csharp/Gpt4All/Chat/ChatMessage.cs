using System.Text.Json.Serialization;

namespace Gpt4All.Chat;

/// <summary>
/// Represents a chat message
/// </summary>
public record ChatMessage
{
    /// <summary>
    /// The role of the author of this message
    /// </summary>
    [JsonPropertyName("role")]
    public ChatRole AuthorRole { get; init; }

    /// <summary>
    /// The content of the message
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; init; }

    public ChatMessage(ChatRole authorRole, string content)
    {
        AuthorRole = authorRole;
        Content = content;
    }
}
