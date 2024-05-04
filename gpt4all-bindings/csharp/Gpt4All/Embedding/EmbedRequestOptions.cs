namespace Gpt4All;

/// <summary>
/// Parameters for embedding requests
/// </summary>
public record EmbedRequestOptions
{
    /// <summary>
    /// The model-specific prefix representing the embedding task, without the trailing colon. NULL for no prefix
    /// </summary>
    /// <remarks>
    /// The model-specific prefix representing the embedding task, without the trailing colon. For Nomic
    /// Embed, this can be `search_query`, `search_document`, `classification`, or `clustering`. Defaults to
    /// `search_document` or equivalent if known; otherwise, you must explicitly pass a prefix or an empty
    /// string if none applies.
    /// </remarks>
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
