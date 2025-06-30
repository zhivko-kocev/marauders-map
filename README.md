<div align="center">

# 🧭 Kocew.Marauders-Map

**A lightweight, region-based navigation system for WPF applications**

[![NuGet](https://img.shields.io/nuget/v/kocew.marauders-map?style=flat-square&logo=nuget)](https://www.nuget.org/packages/kocew.marauders-map)
[![Downloads](https://img.shields.io/nuget/dt/kocew.marauders-map?style=flat-square)](https://www.nuget.org/packages/kocew.marauders-map)
[![License](https://img.shields.io/badge/license-MIT-blue?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-6.0%2B-purple?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)

_Built with CommunityToolkit.Mvvm for modern MVVM development_

[Quick Start](#-quick-setup) • [Documentation](#-notes) • [Examples](#-try-it-out) • [License](#-license)

</div>

---

## ✨ Overview

**Kocew.Marauders-Map** simplifies navigation in WPF by providing a region-based system to manage views via view models and named regions. It leverages `WeakReferenceMessenger` to decouple navigation logic, helping you build clean and modular MVVM applications effortlessly.

### 🎯 Key Features

- 🔄 **Region-based Navigation** - Organize your app into logical regions
- 🧩 **MVVM-First** - Built specifically for MVVM pattern with CommunityToolkit
- 🔗 **Loosely Coupled** - Uses WeakReferenceMessenger for clean separation
- ⚡ **Zero Boilerplate** - Minimal setup, maximum productivity
- 🎨 **Declarative** - Define navigation directly in XAML

---

## 🚀 Quick Setup

### 1️⃣ Install the NuGet Package

```bash
dotnet add package kocew.marauders-map
```

### 2️⃣ Register Views and ViewModels

In your `App.xaml.cs`, register your view models, views, and the navigation system:

```csharp
public partial class App
{
    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        var services = new ServiceCollection();

        // Register ViewModels
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<NestedFirstViewModel>();
        services.AddSingleton<NestedSecondViewModel>();

        // Register Views
        services.AddSingleton<HomeView>();
        services.AddSingleton<SettingsView>();
        services.AddSingleton<NestedFirstView>();
        services.AddSingleton<NestedSecondView>();

        services.AddSingleton<MainWindow>();

        Services = services.BuildServiceProvider();
        NavigationProvider.Services = Services;

        Services.GetRequiredService<MainWindow>().Show();
    }
}
```

### 3️⃣ Define Navigation Region

In your `MainWindow.xaml`, declare a ContentControl with the region name and startup view model:

```xml
<Window
    xmlns:maraudersServices="clr-namespace:Kocew.WPF.MaraudersMap.MaraudersServices;assembly=Kocew.WPF.MaraudersMap"
    ... >

    <ContentControl
        maraudersServices:NavigationRegion.RegionName="MainContent"
        maraudersServices:NavigationRegion.StartupViewModel="{x:Type home:HomeViewModel}" />

</Window>
```

### 4️⃣ Setup Navigation Waypoints

In your views, define navigation waypoints with attached properties:

```xml
<UserControl
    xmlns:maraudersServices="clr-namespace:Kocew.WPF.MaraudersMap.MaraudersServices;assembly=Kocew.WPF.MaraudersMap"
    ... >

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition />
            <RowDefinition />
        </Grid.RowDefinitions>

        <StackPanel>
            <TextBlock Text="{Binding Title}" FontSize="20" Margin="10" />

            <!-- Navigate to Settings in MainContent region -->
            <Button
                Content="Settings"
                maraudersServices:NavigationWaypoint.ViewModel="{x:Type root:SettingsViewModel}"
                maraudersServices:NavigationWaypoint.Region="MainContent" />

            <!-- Navigate to nested views in InnerContent region -->
            <Button
                Content="First Nested"
                maraudersServices:NavigationWaypoint.Region="InnerContent"
                maraudersServices:NavigationWaypoint.ViewModel="{x:Type home:NestedFirstViewModel}" />

            <Button
                Content="Second Nested"
                maraudersServices:NavigationWaypoint.Region="InnerContent"
                maraudersServices:NavigationWaypoint.ViewModel="{x:Type home:NestedSecondViewModel}" />
        </StackPanel>

        <!-- Nested region for inner navigation -->
        <ContentControl
            maraudersServices:NavigationRegion.RegionName="InnerContent"
            Grid.Row="1" />
    </Grid>

</UserControl>
```

> 🎉 **That's it!** All navigation logic happens automatically behind the scenes — no command bindings required.

---

## 💡 Key Concepts

| Concept                | Description                                                  |
| ---------------------- | ------------------------------------------------------------ |
| **NavigationRegion**   | Defines a content area that can display different views      |
| **NavigationWaypoint** | Attached property for buttons/controls to trigger navigation |
| **Region Names**       | String identifiers for different navigation regions          |
| **Startup ViewModel**  | The initial view(model) displayed when the application loads |

### 📋 Requirements

- ✅ Register all Views and ViewModels with your DI container
- ✅ Use `NavigationRegion.RegionName` to mark content areas
- ✅ Use `NavigationRegion.StartupViewModel` for default view on startup (must have one)
- ✅ Use `NavigationWaypoint.Region` to tell where will the button navigate to
- ✅ Use `NavigationWaypoint.ViewModel` to tell which view model to bind to the view
- ✅ Navigation uses `WeakReferenceMessenger` internally for loose coupling

---

## 🗺️ Roadmap

Track our progress and upcoming features:

| Status | Feature                                       | Description                                |
| ------ | --------------------------------------------- | ------------------------------------------ |
| ✅     | **Region-based content control**              | Core navigation system with named regions  |
| ✅     | **Convention-based ViewModel ↔ View mapping** | Automatic view resolution from view models |
| 🔜     | **Parameter passing**                         | Pass data between views during navigation  |
| 🔜     | **Navigation history**                        | Back/forward navigation with history stack |
| 🔜     | **Lifecycle events**                          | `OnNavigatedTo`, `OnNavigatedFrom` hooks   |
| 🔜     | **Animated transitions**                      | Smooth animations between view changes     |

> 💡 **Have a feature request?** [Open an issue](https://github.com/zhivko-kocev/marauders-map/issues) and let us know what you'd like to see!

---

## 📁 Suggested Project Structure

```
YourApp/
├── 📁 Views/
│   ├── 🖼️ HomeView.xaml
│   ├── 🖼️ SettingsView.xaml
│   └── 🖼️ ...
├── 📁 ViewModels/
│   ├── 🧠 HomeViewModel.cs
│   ├── 🧠 SettingsViewModel.cs
│   └── 🧠 ...
├── 📄 App.xaml.cs
├── 🪟 MainWindow.xaml
└── 📄 ...
```

---

## 🧪 Try It Out

Want to see it in action? Check out our example project:

[![Test Project](https://img.shields.io/badge/🚀_Try_Demo-blue?style=for-the-badge)](https://github.com/your-username/marauders-test-project)

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

<div align="center">

**Made with ❤️ for the WPF community**

[![GitHub stars](https://img.shields.io/github/stars/zhivko-kocev/marauders-map?style=social)](https://github.com/zhivko-kocev/marauders-map)

</div>

