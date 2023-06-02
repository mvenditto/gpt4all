namespace Gpt4All.Prediction;

/// <summary>
/// Stats related to a prediction
/// </summary>
public class PredictionInfo
{
    public int PromptTokens { get; init; }

    public int CompletionTokens { get; init; }

    public int TotalTokens => PromptTokens + CompletionTokens;

    public static PredictionInfo Empty => new();
}
