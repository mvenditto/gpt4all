using System.Text.Json.Serialization;

namespace Gpt4All;

/// <summary>
/// Stats related to a prediction
/// </summary>
public class PredictionInfo
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens => PromptTokens + CompletionTokens;

    public static PredictionInfo Empty => new();
}
