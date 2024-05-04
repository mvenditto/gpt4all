using System.Buffers;

namespace Gpt4All;

/// <summary>
/// Represents the result of an embedding request
/// </summary>
public interface IEmbedResult : IDisposable
{
    /// <summary>
    /// Return a sequence that is a view over the generated embeddings
    /// </summary>
    /// <remarks>
    /// The sequence is only valid until the parent <see cref="IEmbedResult"/> is.
    /// The sequence will contain a number of segments equal to the number of texts passed as input to the embedding request.
    /// </remarks>
    ReadOnlySequence<float> GetEmbeddingsSequence();

    /// <summary>
    /// The number of prompt tokens processed or null
    /// </summary>
    public int? TokenCount { get; }
}
