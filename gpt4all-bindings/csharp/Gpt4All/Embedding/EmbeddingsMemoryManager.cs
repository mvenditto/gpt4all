using System.Buffers;
using Gpt4All.Bindings;

namespace Gpt4All;

/// <summary>
/// Handles a native embeddings vector memory
/// </summary>
internal sealed unsafe class EmbeddingsMemoryManager : MemoryManager<float>
{
    private readonly float* _pointer;
    private readonly int _length;
    private bool _disposed;

    internal EmbeddingsMemoryManager(float* pointer, int length)
    {
        ArgumentNullException.ThrowIfNull(pointer, nameof(pointer));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(length, 0, nameof(length));
        _pointer = pointer;
        _length = length;
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        try
        {
            if (disposing)
            {
                NativeMethods.llmodel_free_embedding(_pointer);
            }
        }
        finally
        {
            _disposed = true;
        }
    }

    public override Span<float> GetSpan()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return new Span<float>(_pointer, _length);
    }

    public override MemoryHandle Pin(int elementIndex = 0)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        throw new NotSupportedException();
    }

    public override void Unpin()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}
