using System.Buffers;

namespace Gpt4All;

public record EmbedResult : IEmbedResult
{
    private readonly IMemoryOwner<float> _memoryManager;
    private readonly int _embeddingSize;
    private readonly int _numSegments;
    private readonly int? _promptTokenCount;
    private bool _disposed;

    public int? TokenCount => _promptTokenCount;

    internal unsafe EmbedResult(
        float* embeddingsPtr,
        int numSegments,
        int embeddingSize,
        int? tokenCount)
    {
        _numSegments = numSegments;
        _embeddingSize = embeddingSize;
        _promptTokenCount = tokenCount;
        _memoryManager = new EmbeddingsMemoryManager(embeddingsPtr, embeddingSize);
    }

    public ReadOnlySequence<float> GetEmbeddingsSequence()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_numSegments == 1)
        {
            return new ReadOnlySequence<float>(_memoryManager.Memory);
        }
        else
        {
            var segmentLen = _embeddingSize / _numSegments;
            var memory = _memoryManager.Memory;
            var first = new Embedding(memory[0..segmentLen]);
            var current = first;

            for (var i = 1; i < _numSegments; i++)
            {
                var start = i * segmentLen;
                var end = start + segmentLen;
                current = current.Append(memory[start..end]);
            }

            return new ReadOnlySequence<float>(first, 0, current, current.Memory.Length);
        }
    }

    /// <summary>
    /// Releases the native embeddings vector
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "called in Cleanup()")]
    public void Dispose()
    {
        _disposed = true;
        _memoryManager.Dispose();
    }

    private sealed class Embedding : ReadOnlySequenceSegment<float>
    {
        public Embedding(ReadOnlyMemory<float> memory)
        {
            Memory = memory;
        }

        public Embedding Append(ReadOnlyMemory<float> memory)
        {
            var nextChunk = new Embedding(memory)
            {
                RunningIndex = RunningIndex + Memory.Length
            };

            Next = nextChunk;

            return nextChunk;
        }
    }
}
