using System.Diagnostics.CodeAnalysis;

namespace Kocew.WPF.MaraudersMap.Exceptions;

internal class MaraudersException(string message) : Exception(message)
{
    internal static void ThrowIfNull([NotNull] object? obj, string message)
    {
        if (obj == null)
            throw new MaraudersException(message);
    }
}