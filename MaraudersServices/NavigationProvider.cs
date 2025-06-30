namespace Kocew.WPF.MaraudersMap.MaraudersServices;

/// <summary>
/// Provides a static reference to the application's <see cref="IServiceProvider"/> instance.
/// This class acts as a central point for accessing dependency injection services, which are
/// essential for resolving view models, views, and other services within the application.
/// </summary>
public static class NavigationProvider
{
    /// <summary>
    /// Gets or sets the <see cref="IServiceProvider"/> instance used for dependency injection.
    /// This property is required to resolve services (such as view models and views) from the 
    /// application's dependency injection container.
    /// </summary>
    public static IServiceProvider Services { get; set; } = null!;
}