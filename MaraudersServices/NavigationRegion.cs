using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Kocew.WPF.MaraudersMap.MaraudersEntities;
using Kocew.WPF.MaraudersMap.MarauderUtils;

namespace Kocew.WPF.MaraudersMap.MaraudersServices;

/// <summary>
/// Provides attached properties and logic for managing regions within a WPF application.
/// A region represents a part of the UI (typically a content control area) where specific view models
/// can be navigated to and displayed. This class supports navigation by allowing the definition 
/// of regions and startup view models within those regions.
/// </summary>
public static class NavigationRegion
{
    /// <summary>
    /// DependencyProperty for the RegionName attached property, used to define a named region 
    /// in the UI where view models can be navigated.
    /// </summary>
    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(NavigationRegion),
            new PropertyMetadata(null, OnRegionNameChanged));

    /// <summary>
    /// Sets the RegionName attached property for a given element.
    /// This property associates the element with a named region.
    /// </summary>
    /// <param name="element">The element to set the RegionName for.</param>
    /// <param name="value">The region name to associate with the element.</param>
    public static void SetRegionName(DependencyObject element, string value) =>
        element.SetValue(RegionNameProperty, value);

    /// <summary>
    /// Gets the RegionName attached property from a given element.
    /// </summary>
    /// <param name="element">The element from which to get the RegionName.</param>
    /// <returns>The region name associated with the element.</returns>
    public static string GetRegionName(DependencyObject element) =>
        (string)element.GetValue(RegionNameProperty);

    /// <summary>
    /// DependencyProperty for the StartupViewModel attached property, used to define the 
    /// view model that should be displayed when the region is initialized.
    /// </summary>
    public static readonly DependencyProperty StartupViewModelProperty =
        DependencyProperty.RegisterAttached(
            "StartupViewModel",
            typeof(Type),
            typeof(NavigationRegion),
            new PropertyMetadata(null, OnStartupViewModelChanged));

    /// <summary>
    /// Sets the StartupViewModel attached property for a given element.
    /// This property defines the view model to be displayed at startup in the region.
    /// </summary>
    /// <param name="element">The element to set the StartupViewModel for.</param>
    /// <param name="value">The type of the view model to display in the region.</param>
    public static void SetStartupViewModel(DependencyObject element, Type value) =>
        element.SetValue(StartupViewModelProperty, value);

    /// <summary>
    /// Gets the StartupViewModel attached property from a given element.
    /// </summary>
    /// <param name="element">The element from which to get the StartupViewModel.</param>
    /// <returns>The type of the startup view model for the region.</returns>
    public static Type? GetStartupViewModel(DependencyObject element) =>
        (Type?)element.GetValue(StartupViewModelProperty);

    /// <summary>
    /// Callback method that is invoked when the RegionName attached property is changed.
    /// Registers a message listener that updates the content of the region when a 
    /// <see cref="NavigateMessage"/> is received for the region.
    /// </summary>
    /// <param name="d">The dependency object where the property is being set.</param>
    /// <param name="e">Event arguments containing the new value for the property.</param>
    private static void OnRegionNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ContentControl control)
            throw new InvalidOperationException($"The element {d.GetType()} must be a ContentControl.");

        var region = e.NewValue as string;
        if (string.IsNullOrWhiteSpace(region))
            throw new InvalidOperationException($"The region name '{region}' is empty or null.");

        WeakReferenceMessenger.Default.Register<NavigateMessage>(control, (_, msg) =>
        {
            if (msg.Region == region)
            {
                control.Content = ViewLocator.Resolve(msg.ViewModelType);
            }
        });
    }

    /// <summary>
    /// Callback method that is invoked when the StartupViewModel attached property is changed.
    /// Sends a <see cref="NavigateMessage"/> to trigger the navigation to the specified view model 
    /// in the region defined by the RegionName property.
    /// </summary>
    /// <param name="d">The dependency object where the property is being set.</param>
    /// <param name="e">Event arguments containing the new value for the property.</param>
    private static void OnStartupViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ContentControl control)
            throw new InvalidOperationException($"The element {d.GetType()} must be a ContentControl.");

        if (GetStartupViewModel(control) is not { } viewModel)
            throw new InvalidOperationException($"The StartupViewModel property is not a valid Type");

        var regionName = GetRegionName(control);
        if (string.IsNullOrWhiteSpace(regionName))
            throw new InvalidOperationException($"The region name '{regionName}' is empty or null.");

        WeakReferenceMessenger.Default.Send(new NavigateMessage(viewModel, regionName));
    }
}
