namespace Kocew.WPF.MaraudersMap.MaraudersUtils;

internal class MaraudersServiceProvider
{
    internal static IServiceProvider Instance { get; private set; } = null!;
    
    internal static void Initialize(IServiceProvider provider)
    {
        Instance = provider ?? throw new ArgumentNullException(nameof(provider));
    }
}