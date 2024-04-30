namespace Gpt4All;

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
    /// The embedding dimension, for use with Matryoshka-capable models. Set to -1 to for full-size
    /// </summary>
    public int Dimensionality { get; init; } = -1;

    /// <summary>
    /// True to average multiple embeddings if the text is longer than the model can accept, False to truncate
    /// </summary>
    public bool DoMean { get; init; }

    /// <summary>
    /// Try to be fully compatible with the Atlas API
    /// </summary>
    public bool EnforceAtlasApiCompatibility { get; init; }
}
