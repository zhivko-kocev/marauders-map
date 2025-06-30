Certainly! Here's the updated and cleaned-up **README** for the **`kocew.marauders-map`** package, incorporating the new steps and project details. I've renamed the package and made the necessary adjustments:

---

<div align="center">

# 🧭 **Kocew\.Marauders-Map**

**A lightweight, region-based navigation system for WPF applications**
Built with `CommunityToolkit.Mvvm` for modern MVVM development.

[![NuGet](https://img.shields.io/nuget/v/kocew.marauders-map.svg)](https://www.nuget.org/packages/kocew.marauders-map)
[![Downloads](https://img.shields.io/nuget/dt/kocew.marauders-map.svg)](https://www.nuget.org/packages/kocew.marauders-map)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

</div>

---

## ✅ Overview

`Kocew.Marauders-Map` is a simple, region-based navigation system for WPF applications. It helps you design modular, clean MVVM applications by enabling navigation between views using named regions and message-passing. With this package, navigating through your application becomes effortless and intuitive.

---

## 🚀 Quick Setup

### 1. Install the NuGet Package

```bash
dotnet add package kocew.marauders-map
```

---

### 2. Register Views and ViewModels

In your `App.xaml.cs` (or `App()` constructor), register all the necessary ViewModels and Views, as well as the navigation system:

```csharp
var services = new ServiceCollection();

// Register all ViewModels
services.AddSingleton<HomeViewModel>();
services.AddSingleton<SettingsViewModel>();
services.AddSingleton<NestedFirstViewModel>();
services.AddSingleton<NestedSecondViewModel>();

// Register all Views
services.AddSingleton<HomeView>();
services.AddSingleton<SettingsView>();
services.AddSingleton<NestedFirstView>();
services.AddSingleton<NestedSecondView>();
services.AddSingleton<MainWindow>();

// Register the navigation system
services.AddMaraudersMap();

// Build the service provider and set it
Services = services.BuildServiceProvider();
NavigationProvider.Services = Services;

// Show main window
Services.GetRequiredService<MainWindow>().Show();
```

---

### 3. Define Navigation Region in Your Window

In the `MainWindow.xaml`, define a **`ContentControl`** for the navigation region and bind it to a named region:

```xml
<Window xmlns:map="clr-namespace:Kocew.MaraudersMap;assembly=Kocew.Marauders-Map" ...>
    <!-- The Name must match the region name you navigate to -->
    <ContentControl map:MarauderNavRegion.Name="HomeView" map:MarauderNavRegion.IsDefault="True"/>
</Window>
```

---

### 4. Setup Nested Regions in Views (Optional)

For nested regions, add **`ContentControl`** elements within your `UserControl` to represent different sections of the UI that will be navigated to separately.

```xml
<UserControl ... xmlns:map="clr-namespace:Kocew.MaraudersMap;assembly=Kocew.Marauders-Map">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition />
            <RowDefinition />
        </Grid.RowDefinitions>

        <StackPanel>
            <TextBlock Text="{Binding Title}" FontSize="20" Margin="10"/>
            <Button Content="Settings" Command="{Binding NavigateCommand}" CommandParameter="{x:Type root:SettingsViewModel}" />
            <Button Content="FirstNested" Command="{Binding NestedNavigateCommand}" CommandParameter="{x:Type nested:NestedFirstViewModel}" />
            <Button Content="SecondNested" Command="{Binding NestedNavigateCommand}" CommandParameter="{x:Type nested:NestedSecondViewModel}" />
        </StackPanel>

        <!-- Nested region -->
        <ContentControl Grid.Row="1" map:MarauderNavRegion.Name="InnerContent"/>
    </Grid>
</UserControl>
```

---

### 5. Navigate from ViewModel

In your **ViewModel**, use `WeakReferenceMessenger` to send **navigation messages** to navigate between views or regions.

```csharp
public partial class HomeViewModel : ObservableRecipient
{
    public string Title => "Welcome to Home View!";

    [RelayCommand]
    private static void Navigate(Type viewModel)
    {
        WeakReferenceMessenger.Default.Send(new NavigateMessage(viewModel, "HomeView"));
    }

    [RelayCommand]
    private static void NestedNavigate(Type viewModel)
    {
        WeakReferenceMessenger.Default.Send(new NavigateMessage(viewModel, "InnerContent"));
    }
}
```

---

## 💡 Notes

- ViewModels and Views must be registered in the **Dependency Injection (DI)** container.
- Regions are identified by **name**. If no region name is explicitly provided, the default region will be used.
- Use **`WeakReferenceMessenger`** to trigger navigation messages throughout your application.
- The `MarauderNavRegion` control will automatically handle view switching based on messages sent.

---

## 📁 Suggested Folder Structure

```plaintext
YourApp/
├── Views/
│   ├── HomeView.xaml
│   ├── SettingsView.xaml
│   └── ...
├── ViewModels/
│   ├── HomeViewModel.cs
│   ├── SettingsViewModel.cs
│   └── ...
├── App.xaml.cs
├── MainWindow.xaml
└── ...
```

---

## 📌 Roadmap

- ✅ Region-based content control
- ✅ Convention-based ViewModel ↔ View mapping
- 🔜 Navigation history
- 🔜 Lifecycle events (OnNavigatedTo, OnNavigatedFrom)
- 🔜 Parameter passing
- 🔜 Animated transitions

---

## 📄 License

Licensed under the [MIT License](LICENSE).

---

<div align="center">Made with ❤️ for the WPF community</div>

---
