namespace System.Collections.Generic;

internal static class IEnumerableExtensions
{
    public static string Join<T>(this IEnumerable<T> source, string separator = ",")
    {
        ArgumentNullException.ThrowIfNull(separator);

        if (source?.Any() != true)
        {
            return "[ ]";
        }

        return "[" + string.Join(separator, source) + "]";
    }
}
