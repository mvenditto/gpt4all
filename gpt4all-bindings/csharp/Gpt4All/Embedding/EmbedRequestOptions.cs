namespace Gpt4All;

/// <summary>
/// Modes to instruct the model how to handle texts longer than it can accept
/// </summary>
public enum LongTextMode
{
    /// <summary>
    /// Average multiple embeddings
    /// </summary>
    Mean = 1,
    /// <summary>
    /// Truncate
    /// </summary>
    Truncate = 2
}

/// <summary>
/// Parameters for embedding requests
/// </summary>
public record EmbedRequestOptions
{
    /// <summary>
    /// The model-specific prefix representing the embedding task, without the trailing colon. NULL for no prefix
    /// </summary>
    public string? Prefix { get; init; }

    /// <summary>
    /// The embedding dimension, for use with Matryoshka-capable models. Set to null for full-size
    /// </summary>
    public int? Dimensionality { get; init; }

    /// <summary>
    /// How to handle texts longer than the model can accept
    /// </summary>
    public LongTextMode LongTextMode { get; init; } = LongTextMode.Truncate;

    /// <summary>
    /// Try to be fully compatible with the Atlas API
    /// </summary>
    public bool EnforceAtlasApiCompatibility { get; init; }
}
