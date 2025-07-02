using Microsoft.Extensions.DependencyInjection;

namespace Kocew.WPF.MaraudersMap.MaraudersUtils;

public static class ServiceCollectionExtensions
{
    public static void AddMaraudersMap(this IServiceCollection services)
    {
        services.AddSingleton<ViewLocator>();
    }

    public static IServiceProvider BuildMaraudersMap(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        MaraudersServiceProvider.Initialize(provider);

        return provider;
    }
}