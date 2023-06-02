using Gpt4All.Chat;

namespace Gpt4All;

/// <summary>
/// Formats a prompt
/// </summary>
public interface IPromptFormatter
{
    /// <summary>
    /// Format the provided prompt
    /// </summary>
    /// <param name="prompt">the input prompt</param>
    /// <returns>The formatted prompt</returns>
    string FormatPrompt(string prompt);

    /// <summary>
    /// Format the provided chat history
    /// </summary>
    /// <param name="chat">the chat history to format</param>
    /// <returns>The formatted prompt</returns>
    string FormatChatPrompt(IChatConversation chat);
}
