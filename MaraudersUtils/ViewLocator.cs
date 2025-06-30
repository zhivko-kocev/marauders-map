using System.Windows.Controls;
using Kocew.WPF.MaraudersMap.MaraudersServices;
using Microsoft.Extensions.DependencyInjection;

namespace Kocew.WPF.MaraudersMap.MaraudersUtils;

/// <summary>
/// Resolves Views from ViewModels using naming conventions.
/// This class helps in locating and creating views based on the associated view model types.
/// It uses the full name of the view model to find the corresponding view by adhering to a naming convention.
/// </summary>
internal static class ViewLocator
{
    /// <summary>
    /// Resolves and creates the corresponding view for the given view model type.
    /// The view is located by replacing parts of the view model's name based on a naming convention:
    /// - "ViewModels" is replaced with "Views"
    /// - "ViewModel" is replaced with "View"
    /// The resolved view is then instantiated, and the view model is set as its data context.
    /// </summary>
    /// <param name="viewModelType">The type of the view model to resolve the corresponding view for.</param>
    /// <returns>A <see cref="UserControl"/> instance that corresponds to the given view model type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="viewModelType"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the corresponding view cannot be found, 
    /// or if the resolved view type is not a <see cref="UserControl"/>.</exception>
    public static UserControl Resolve(Type viewModelType)
    {
        ArgumentNullException.ThrowIfNull(viewModelType);
        ArgumentNullException.ThrowIfNull(nameof(NavigationProvider.Services));

        // Resolve the view model instance from the dependency injection container
        var viewModel = NavigationProvider.Services.GetRequiredService(viewModelType);

        // Generate the view name by replacing naming conventions
        var viewName = viewModelType.FullName?
            .Replace("ViewModels", "Views")
            .Replace("ViewModel", "View");

        if (viewName == null)
            throw new InvalidOperationException(
                $"Cannot resolve view name from ViewModel type: {viewModelType.FullName}");

        // Search for the view type by matching the view name
        var viewType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.FullName == viewName);

        if (viewType == null)
            throw new InvalidOperationException($"Could not find View type: {viewName}");

        // Resolve the view from the DI container
        if (NavigationProvider.Services.GetRequiredService(viewType) is not UserControl view)
            throw new InvalidOperationException($"Resolved type is not a UserControl: {viewType}");

        // Set the data context of the view to the resolved view model
        view.DataContext = viewModel;
        return view;
    }
}
