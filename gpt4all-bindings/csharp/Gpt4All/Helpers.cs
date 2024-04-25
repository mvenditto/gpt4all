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
}
