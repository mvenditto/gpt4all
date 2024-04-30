namespace Gpt4All;

/// <summary>
/// Represents the result of an embedding request
/// </summary>
public interface IEmbedResult : IDisposable
{
    /// <summary>
    /// Gets the embeddings as a readonly span of floats
    /// </summary>
    ReadOnlySpan<float> AsSpan();

    /// <summary>
    /// Gets the embeddings as a readonly memory of floats
    /// </summary>
    ReadOnlyMemory<float> AsMemory();

    /// <summary>
    /// The number of embeddings in the result
    /// </summary>
    int EmbeddingSize { get; }
}
