using Gpt4All.Bindings;

namespace Gpt4All.Embedding;

public unsafe class TextEmbeddingResult : ITextEmbeddingResult
{
    private readonly float* _embeddingsPointer;
    private readonly nuint _embeddingsLength;
    private bool _disposed;

    internal TextEmbeddingResult(float* embeddings, nuint embeddingsLength)
    {
        _embeddingsPointer = embeddings;
        _embeddingsLength = embeddingsLength;
    }

    public Span<float> Embeddings
    {
        get
        {
            if (_disposed) throw new ObjectDisposedException(string.Empty);

            return new(_embeddingsPointer, (int)_embeddingsLength);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
        }

        NativeMethods.llmodel_free_embedding(_embeddingsPointer);

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
