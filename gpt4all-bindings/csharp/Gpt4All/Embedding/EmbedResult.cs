using System.Buffers;

namespace Gpt4All;

public record EmbedResult : IEmbedResult
{
    private readonly IMemoryOwner<float> _memoryManager;

    internal unsafe EmbedResult(float* embeddingsPtr, int embeddingSize)
    {
        EmbeddingSize = embeddingSize;
        _memoryManager = new EmbeddingsMemoryManager(embeddingsPtr, embeddingSize);
    }

    /// <inheritdoc/>
    public int EmbeddingSize { get; }

    /// <inheritdoc/>
    public ReadOnlyMemory<float> AsMemory()
    {
        return _memoryManager.Memory;
    }

    /// <inheritdoc/>
    public ReadOnlySpan<float> AsSpan()
    {
        return _memoryManager.Memory.Span;
    }

    /// <summary>
    /// Releases the native embeddings vector
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "called in Cleanup()")]
    public void Dispose()
    {
        _memoryManager.Dispose();
    }
}
