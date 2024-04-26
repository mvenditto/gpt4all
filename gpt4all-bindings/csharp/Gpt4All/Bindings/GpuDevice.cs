namespace Gpt4All.Bindings;

public class GpuDevice
{
    public int Index { get; init; }

    public int Type { get; init; }

    public nuint HeapSize { get; init; }

    public string Name { get; init; } = null!;

    public string Vendor { get; init; } = null!;
}
