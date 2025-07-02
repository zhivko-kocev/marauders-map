using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Kocew.WPF.MaraudersMap.Exceptions;
using Kocew.WPF.MaraudersMap.MaraudersEntities;
using Kocew.WPF.MaraudersMap.MaraudersUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Kocew.WPF.MaraudersMap.MaraudersServices;

public class NavigationRegion
{
    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(NavigationRegion),
            new PropertyMetadata(null, OnRegionNameChanged));

    public static void SetRegionName(DependencyObject element, string value) =>
        element.SetValue(RegionNameProperty, value);
    public static string GetRegionName(DependencyObject element) =>
        (string)element.GetValue(RegionNameProperty);

    public static readonly DependencyProperty StartupViewModelProperty =
        DependencyProperty.RegisterAttached(
            "StartupViewModel",
            typeof(Type),
            typeof(NavigationRegion),
            new PropertyMetadata(null, OnStartupViewModelChanged));

    public static void SetStartupViewModel(DependencyObject element, Type value) =>
        element.SetValue(StartupViewModelProperty, value);
    public static Type? GetStartupViewModel(DependencyObject element) =>
        (Type?)element.GetValue(StartupViewModelProperty);

    private static void OnRegionNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ContentControl control)
            throw new MaraudersException($"The element {d.GetType()} must be a ContentControl.");

        var region = e.NewValue as string;
        if (string.IsNullOrWhiteSpace(region))
            throw new MaraudersException($"The region name '{region}' is empty or null.");

        WeakReferenceMessenger.Default.Register<NavigateMessage>(control, (_, msg) =>
        {
            if (msg.Region != region) return;

            Type? sender = null;
            Dictionary<string, object> data = [];

            if (control.Content is Control { DataContext: IMaraudersAware vmf })
            {
                data = vmf.OnNavigatedFrom();
                sender = vmf.GetType();
            }

            sender ??= typeof(Application);
            
            UserControl view = MaraudersServiceProvider
                .Instance
                .GetRequiredService<ViewLocator>()
                .Resolve(msg.ViewModelType);
            control.Content = view;
            
            if (control.Content is Control { DataContext: IMaraudersAware vmt })
                vmt.OnNavigatedTo(sender!, data);
        });
    }

    private static void OnStartupViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ContentControl control)
            throw new MaraudersException($"The element {d.GetType()} must be a ContentControl.");

        if (e.NewValue is not Type viewModel)
            throw new MaraudersException($"The StartupViewModel in the {control.GetType()} must be set.");

        var regionName = GetRegionName(control);
        if (string.IsNullOrWhiteSpace(regionName))
            throw new MaraudersException($"The region name '{regionName}' is empty or null.");

        WeakReferenceMessenger.Default.Send(new NavigateMessage(viewModel, regionName));
    }
}