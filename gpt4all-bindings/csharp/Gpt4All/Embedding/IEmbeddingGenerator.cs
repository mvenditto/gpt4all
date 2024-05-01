namespace Gpt4All;

/// <summary>
/// Represents an embedding generator
/// </summary>
public interface IEmbeddingGenerator
{
    /// <summary>
    /// Generates embeddings for the given texts
    /// </summary>
    /// <param name="texts">The input texts</param>
    /// <param name="opts">The embedding generation options</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The generated embeddings</returns>
    Task<IEmbedResult> GetEmbeddingsAsync(
        string[] texts,
        EmbedRequestOptions opts,
        CancellationToken cancellationToken = default);
}
