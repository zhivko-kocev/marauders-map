using System.Windows.Controls;
using Kocew.WPF.MaraudersMap.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Kocew.WPF.MaraudersMap.MaraudersUtils;

internal class ViewLocator(IServiceProvider services)
{
    private readonly Dictionary<string, Type> _cache = new();

    internal UserControl Resolve(Type viewModelType)
    {
        ArgumentNullException.ThrowIfNull(viewModelType, nameof(viewModelType));
        
        var viewModel = services.GetRequiredService(viewModelType);
        MaraudersException.ThrowIfNull(viewModel, $"Please register the ViewModel {viewModelType.FullName}");

        var viewName = ResolveViewName(viewModelType);
        var viewType = ResolveViewType(viewName, viewModelType.Assembly.GetTypes());

        if (services.GetRequiredService(viewType) is not UserControl view)
            throw new MaraudersException($"Resolved type is not a UserControl: {viewType}");

        view.DataContext = viewModel;
        return view;
    }

    private Type ResolveViewType(string viewName, Type[] types)
    {
        if (_cache.TryGetValue(viewName, out var cachedViewType))
            return cachedViewType;

        var viewType = types.FirstOrDefault(type => type.FullName == viewName);
        MaraudersException.ThrowIfNull(viewType,
            $"Could not find View type: {viewName} in the Views folder or on the same level with the ViewModel");

        _cache[viewName] = viewType;
        return viewType;
    }

    private string ResolveViewName(Type viewModelType)
    {
        MaraudersException.ThrowIfNull(viewModelType.Namespace,
            $"Cannot resolve Namespace for: {viewModelType}");
        
        MaraudersException.ThrowIfNull(viewModelType.FullName, 
            $"Cannot resolve view name for: {viewModelType}");

        return viewModelType
            .Namespace.Contains("ViewModels")
            ? viewModelType.FullName
                .Replace("ViewModel", "View")
            : viewModelType.FullName
                .Replace("ViewModels", "Views")
                .Replace("ViewModel", "View");
    }
}