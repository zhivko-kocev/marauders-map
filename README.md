Thanks! Based on your process and the code you shared, here's a **cleaned-up, professional README refactor** that reflects your actual setup steps with clarity and purpose.

---

<div align="center">

# 🧭 Xrite.Wpf.Navigation

**A lightweight, region-based navigation system for WPF applications**
Built with `CommunityToolkit.Mvvm` for modern MVVM development.

[![NuGet](https://img.shields.io/nuget/v/Xrite.Wpf.Navigation.svg)](https://www.nuget.org/packages/Xrite.Wpf.Navigation)
[![Downloads](https://img.shields.io/nuget/dt/Xrite.Wpf.Navigation.svg)](https://www.nuget.org/packages/Xrite.Wpf.Navigation)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

</div>

---

## ✅ Overview

`Xrite.Wpf.Navigation` is a simple, region-based navigation system for WPF. It allows for modular, clean MVVM application design, enabling navigation between views using named regions and message-passing.

---

## 🚀 Quick Setup

### 1. Install the NuGet Package

```bash
dotnet add package Xrite.Wpf.Navigation
```

---

### 2. Register Views and ViewModels

In your `App.xaml.cs` (or `App()` constructor):

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
services.AddNavigation();

// Build service provider and set it
Services = services.BuildServiceProvider();
NavigationProvider.Services = Services;

// Show main window
Services.GetRequiredService<MainWindow>().Show();
```

---

### 3. Define Navigation Region in Your Window

```xml
<Window xmlns:nav="clr-namespace:Xrite.Wpf.Navigation;assembly=Xrite.Wpf.Navigation" ...>
    <!-- The Name must match the region name you navigate to -->
    <ContentControl nav:NavigationRegion.Name="HomeView" nav:NavigationRegion.IsDefault="True"/>
</Window>
```

---

### 4. Setup Nested Regions in Views (Optional)

```xml
<UserControl ... xmlns:nav="clr-namespace:Xrite.Wpf.Navigation;assembly=Xrite.Wpf.Navigation">
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
        <ContentControl Grid.Row="1" nav:NavigationRegion.Name="InnerContent"/>
    </Grid>
</UserControl>
```

---

### 5. Navigate from ViewModel

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

* ViewModels and Views must be registered in the DI container.
* Regions are identified by name. The default region should match the name you navigate to if not explicitly specified.
* Use `WeakReferenceMessenger` to trigger navigation from anywhere in your app.

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

* ✅ Region-based content control
* ✅ Convention-based ViewModel ↔ View mapping
* 🔜 Navigation history
* 🔜 Lifecycle events (OnNavigatedTo, OnNavigatedFrom)
* 🔜 Parameter passing
* 🔜 Animated transitions

---

## 📄 License

Licensed under the [MIT License](LICENSE).

---

<div align="center">Made with ❤️ for the WPF community</div>

---
