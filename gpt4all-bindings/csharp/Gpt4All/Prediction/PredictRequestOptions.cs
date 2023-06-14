namespace Gpt4All;

/// <summary>
/// Represente the parameters for a text generation request
/// </summary>
public record PredictRequestOptions
{
    /// <summary>
    /// the size of the raw logits vector
    /// </summary>
    public nuint LogitsSize { get; init; } = 0;

    /// <summary>
    /// the size of the raw tokens vector
    /// </summary>
    public nuint TokensSize { get; init; } = 0;

    /// <summary>
    /// number of tokens in past conversation
    /// </summary>
    public int PastConversationTokensNum { get; init; } = 0;

    /// <summary>
    /// number of tokens possible in context window
    /// </summary>
    public int ContextSize { get; init; } = 1024;

    /// <summary>
    /// Number of tokens to predict
    /// </summary>
    public int TokensToPredict { get; init; } = 128;

    /// <summary>
    /// top k logits to sample from
    /// </summary>
    public int TopK { get; init; } = 40;

    /// <summary>
    /// nucleus sampling probability threshold
    /// </summary>
    public float TopP { get; init; } = 0.9f;

    /// <summary>
    /// temperature to adjust model's output distribution
    /// </summary>
    public float Temperature { get; init; } = 0.1f;

    /// <summary>
    /// number of predictions to generate in parallel
    /// </summary>
    public int Batches { get; init; } = 8;

    /// <summary>
    /// penalty factor for repeated tokens
    /// </summary>
    public float RepeatPenalty { get; init; } = 1.2f;

    /// <summary>
    /// last n tokens to penalize
    /// </summary>
    public int RepeatLastN { get; init; } = 10;

    /// <summary>
    /// percent of context to erase if we exceed the context window
    /// </summary>
    public float ContextErase { get; init; } = 0.5f;

    /// <summary>
    /// defaults for text prediction
    /// </summary>
    public static readonly PredictRequestOptions Defaults = new();
}
