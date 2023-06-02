using System.Text;
using Gpt4All.Chat;

namespace Gpt4All;

public class DefaultPromptFormatter : IPromptFormatter
{
    protected string _template = """
        ### Instruction: 
        The prompt below is a question to answer, a task to complete, or a conversation
        to respond to; decide which and write an appropriate response.
        ### Prompt:
        {0}
        ### Response:
        """;

    public string FormatPrompt(string prompt)
    {
        return string.Format(_template, prompt);
    }

    public string FormatChatPrompt(IChatConversation chat)
    {
        var sb = new StringBuilder();

        foreach (var msg in chat.Messages.Where(x => x.AuthorRole == ChatRole.System))
        {
            sb.AppendLine(msg.Content.Trim('\n'));
        }

        foreach (var msg in chat.Messages)
        {
            var text = msg.Content.Trim('\n');
            switch (msg.AuthorRole)
            {
                case ChatRole.Unknown:
                case ChatRole.System:
                    continue;
                case ChatRole.User:
                case ChatRole.Assistant:
                    sb.AppendLine(text);
                    break;
            }
        }

        var fullChatHistory = sb.ToString();

        fullChatHistory = fullChatHistory[^1] == '\n'
            ? fullChatHistory[..^1]
            : fullChatHistory;

        return FormatPrompt(fullChatHistory);
    }
}
