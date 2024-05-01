namespace Gpt4All;

public class ModelCreationException : Exception
{
    public ModelCreationException() : base()
    {
    }

    public ModelCreationException(string? message) : base(message)
    {
    }

    public ModelCreationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
