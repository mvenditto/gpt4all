namespace Gpt4All.Embedding;

/// <summary>
/// Interface for text embedding service
/// </summary>
public interface ITextEmbedding
{
    /// <summary>
    /// Generate an embedding using the model.
    /// </summary>
    /// <param name="text">A string representing the text to generate an embedding for</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"></see></param>
    /// <returns>The generated embedding for the input text</returns>
    Task<ITextEmbeddingResult> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
}
