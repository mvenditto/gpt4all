namespace Gpt4All;

public class ModelLoadException : Exception
{
    public ModelLoadException(string modelPath, ModelType? modelType = null)
        : base($"Unable to load model '{modelPath}' of type '{modelType}')")
    {
    }

    public ModelLoadException()
    {
    }

    public ModelLoadException(string? message) : base(message)
    {
    }

    public ModelLoadException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
