namespace Gpt4All;

public class ModelLoadException : Exception
{
    public ModelLoadException() : base()
    {
    }

    public ModelLoadException(string? message) : base(message)
    {
    }

    public ModelLoadException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
