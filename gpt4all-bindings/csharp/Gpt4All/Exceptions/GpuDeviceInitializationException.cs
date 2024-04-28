namespace Gpt4All;

public class GpuDeviceInitializationException : Exception
{
    public string FailedDeviceName { get; init; } = string.Empty;

    public GpuDeviceInitializationException() : base()
    {
    }

    public GpuDeviceInitializationException(string? message) : base(message)
    {
    }

    public GpuDeviceInitializationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
