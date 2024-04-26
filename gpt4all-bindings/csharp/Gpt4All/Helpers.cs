using System.Runtime.InteropServices;
using Gpt4All.Bindings;

namespace Gpt4All;

public static class Helpers
{
    public static void SetImplementationSearchPath(string searchPath)
    {
        NativeMethods.llmodel_set_implementation_search_path(searchPath);
    }

    public static string GetImplementationSearchPath()
    {
        var path = NativeMethods.llmodel_get_implementation_search_path();
        return Marshal.PtrToStringUTF8(path) ?? string.Empty;
    }

    public static IEnumerable<GpuDevice> GetAvailableGpuDevices(nuint minRequiredMemory = 0)
    {
        unsafe
        {
            var devicePtr = NativeMethods.llmodel_available_gpu_devices(minRequiredMemory, out var numDevices);

            if (devicePtr == null)
            {
                throw new Exception("Unable to retrieve available GPU devices"); // TODO: better type for this kind of exception
            }

            var devices = new Span<llmodel_gpu_device>(devicePtr, numDevices);

            var gpuDevices = new List<GpuDevice>(numDevices);

            for (var i = 0; i < numDevices; i++)
            {
                var device = devices[i];

                gpuDevices.Add(new GpuDevice
                {
                    Index = device.index,
                    Type = device.type,
                    HeapSize = device.heapSize,
                    Name = Marshal.PtrToStringUTF8(device.name) ?? string.Empty,
                    Vendor = Marshal.PtrToStringUTF8(device.vendor) ?? string.Empty
                });
            }

            return gpuDevices;
        }
    }
}
