namespace Gpt4All.Embedding;

public interface ITextEmbeddingResult : IDisposable
{
    Span<float> Embeddings { get; }
}
