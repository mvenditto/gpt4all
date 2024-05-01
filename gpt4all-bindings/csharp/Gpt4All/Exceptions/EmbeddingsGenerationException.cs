namespace Gpt4All;

public class EmbeddingsGenerationException : Exception
{
    public EmbeddingsGenerationException() : base()
    {
    }

    public EmbeddingsGenerationException(string? message) : base(message)
    {
    }

    public EmbeddingsGenerationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
