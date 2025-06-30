using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Kocew.WPF.MaraudersMap.MaraudersEntities;

namespace Kocew.WPF.MaraudersMap.MaraudersServices;

public static class NavigationWaypoint
{
    public static readonly DependencyProperty RegionProperty =
        DependencyProperty.RegisterAttached(
            "Region",
            typeof(string),
            typeof(NavigationWaypoint),
            new PropertyMetadata(OnChanged));

    public static string? GetRegion(DependencyObject obj) => (string)obj.GetValue(RegionProperty);
    public static void SetRegion(DependencyObject obj, string value) => obj.SetValue(RegionProperty, value);

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.RegisterAttached(
            "ViewModel",
            typeof(Type),
            typeof(NavigationWaypoint),
            new PropertyMetadata(OnChanged));

    public static Type? GetViewModel(DependencyObject obj) => (Type)obj.GetValue(ViewModelProperty);
    public static void SetViewModel(DependencyObject obj, Type value) => obj.SetValue(ViewModelProperty, value);

    private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Button button)
            throw new InvalidOperationException($"The element {d.GetType()} must be a Button.");

        button.Dispatcher.BeginInvoke(() =>
        {
            if(GetViewModel(button) is not { } viewModel)
                throw new InvalidOperationException($"The StartupViewModel property is not a valid Type");

            var region = GetRegion(button);
            if (string.IsNullOrWhiteSpace(region))
                throw new InvalidOperationException($"The region name '{region}' is empty or null.");
            
            if(button.Command is RelayCommand<object?>) return;

            button.Command = new RelayCommand<object?>(parameter =>
                WeakReferenceMessenger.Default.Send(new NavigateMessage(viewModel, region, parameter))
            );
        });
    }
}