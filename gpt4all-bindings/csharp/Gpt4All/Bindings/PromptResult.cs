namespace Gpt4All.Bindings;

public record PromptResult
{
    public int PromptTokens { get; init; }

    public int CompletionTokens { get; set; }

    public int TotalTokens => PromptTokens + CompletionTokens;
}
