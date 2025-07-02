using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kocew.WPF.MaraudersMap.Exceptions;
using Kocew.WPF.MaraudersMap.MaraudersEntities;

namespace Kocew.WPF.MaraudersMap.MaraudersServices;

public static class NavigationWaypoint
{
    public static readonly DependencyProperty RegionProperty =
        DependencyProperty.RegisterAttached(
            "Region",
            typeof(string),
            typeof(NavigationWaypoint),
            new PropertyMetadata());

    public static string? GetRegion(DependencyObject obj) => (string)obj.GetValue(RegionProperty);
    public static void SetRegion(DependencyObject obj, string value) => obj.SetValue(RegionProperty, value);

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.RegisterAttached(
            "ViewModel",
            typeof(Type),
            typeof(NavigationWaypoint),
            new PropertyMetadata(OnViewModelChanged));

    public static Type? GetViewModel(DependencyObject obj) => (Type)obj.GetValue(ViewModelProperty);
    public static void SetViewModel(DependencyObject obj, Type value) => obj.SetValue(ViewModelProperty, value);

    private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Button button)
            throw new MaraudersException($"The element {d.GetType()} must be a Button.");

        if (e.NewValue is not Type viewModel)
            throw new MaraudersException($"The ViewModel in the {button.GetType()} must be set.");

        var region = GetRegion(button);
        if (string.IsNullOrWhiteSpace(region))
            throw new MaraudersException($"The region name '{region}' is empty or null.");

        if (button.Command is RelayCommand) return;

        button.Command = new RelayCommand(() =>
            WeakReferenceMessenger.Default.Send(new NavigateMessage(viewModel, region))
        );
    }
}