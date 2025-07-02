<div align="center">

# 🧭 Kocew\.Marauders-Map

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

**Kocew\.Marauders-Map** simplifies navigation in WPF by providing a region-based system to manage views via view models and named regions. It leverages `WeakReferenceMessenger` to decouple navigation logic, helping you build clean and modular MVVM applications effortlessly.

### 🎯 Key Features

- 🔄 **Region-based Navigation** - Organize your app into logical regions.
- 🧩 **MVVM-First** - Built specifically for MVVM pattern with CommunityToolkit.
- 🔗 **Loosely Coupled** - Uses WeakReferenceMessenger for clean separation.
- ⚡ **Zero Boilerplate** - Minimal setup, maximum productivity.
- 🎨 **Declarative** - Define navigation directly in XAML.

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

        // Register Marauders Map services
        services.AddMaraudersMap();

        Services = services.BuildMaraudersMap();

        // Launch the MainWindow
        Services.GetRequiredService<MainWindow>().Show();
    }
}
```

### **Important**: The application **must** have one `StartupViewModel` to define the initial view when the application starts. This `StartupViewModel` should be associated with the initial view displayed in the `ContentControl`:

```xml
<ContentControl
    maraudersServices:NavigationRegion.RegionName="MainContent"
    maraudersServices:NavigationRegion.StartupViewModel="{x:Type home:HomeViewModel}" />
```

This will ensure the application launches with the correct initial view model (`HomeViewModel` in this case).

### 3️⃣ Define Navigation Region

In your `MainWindow.xaml`, declare a `ContentControl` with the region name and startup view model. **Note that the region is specified first**:

```xml
<Window
    xmlns:maraudersServices="clr-namespace:Kocew.WPF.MaraudersMap.MaraudersServices;assembly=Kocew.WPF.MaraudersMap"
    ... >

    <!-- Define MainContent region -->
    <ContentControl
        maraudersServices:NavigationRegion.RegionName="MainContent"
        maraudersServices:NavigationRegion.StartupViewModel="{x:Type home:HomeViewModel}" />

</Window>
```

### 4️⃣ Setup Navigation Waypoints

In your views, define navigation waypoints with attached properties. Ensure that **Region comes first** in the XAML:

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
                maraudersServices:NavigationWaypoint.Region="MainContent"
                maraudersServices:NavigationWaypoint.ViewModel="{x:Type root:SettingsViewModel}" />

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

### `MaraudersAware` is Required for Navigation

To enable navigation in **Kocew\.Marauders-Map**, your view models **must inherit from the `MaraudersAware` class**. This class is essential for integrating with the navigation system. Without inheriting from `MaraudersAware`, your view models will not be able to participate in the navigation process.

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using Kocew.WPF.MaraudersMap.MaraudersEntities;
using System;
using System.Collections.Generic;

namespace TryMaraudersProject.ViewModels.Home
{
    // Inheriting from MaraudersAware to enable navigation
    public class HomeViewModel : MaraudersAware
    {

    }
}
```

> 🎉 **That's it!** All navigation logic happens automatically behind the scenes — no command bindings required.

---

## 💡 Lifecycle Events and Content Passing

### The `MaraudersAware` class provides default implementations for the lifecycle events `OnNavigatedTo` and `OnNavigatedFrom`, which **must be overridden** in your view models to handle the custom logic for navigation.

### `OnNavigatedTo` and `OnNavigatedFrom`

- **`OnNavigatedTo`**: This method is called when navigating to a view. It receives two parameters:

  - **`sender`**: The type of the view or view model that navigated to this view.
  - **`content`**: A dictionary containing the data passed from the previous view.

- **`OnNavigatedFrom`**: This method is called when navigating away from a view. It returns a dictionary that can contain data you want to pass to the next view.

### Example:

In the `HomeViewModel`, the view model must inherit from `MaraudersAware` to enable navigation. Here's an example of how lifecycle management and content passing is implemented:

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using Kocew.WPF.MaraudersMap.MaraudersEntities;
using System;
using System.Collections.Generic;

namespace TryMaraudersProject.ViewModels.Home
{
    // Inheriting from MaraudersAware to enable navigation
    public class HomeViewModel : MaraudersAware
    {
        public string Title => "Welcome to Home View!";

        // Called when navigating to this view
        public override void OnNavigatedTo(Type sender, Dictionary<string, object> content)
        {
            // Log sender and content information
            Console.WriteLine($"OnNavigatedTo called from {GetType().Name} and the sender is {sender.Name} and it sent {content.Count} items");

            // Process content passed during navigation
            if (content.ContainsKey("someKey"))
            {
                var someValue = content["someKey"];
                Console.WriteLine($"Received value: {someValue}");
            }
        }

        // Called when navigating away from this view
        public override Dictionary<string, object> OnNavigatedFrom()
        {
            // Log when navigation occurs
            Console.WriteLine($"OnNavigatedFrom called from {GetType().Name}");

            // Prepare content to be passed to the next view
            var content = new Dictionary<string, object>
            {
                { "someKey", "Some Value to Pass" },
                { "anotherKey", 42 }
            };

            // Return the content to be passed to the next view
            return content;
        }
    }
}
```

### Key Points:

- **`MaraudersAware` Base Class**: The `MaraudersAware` class is essential for enabling navigation. Your view models **must inherit from `MaraudersAware`** for the navigation system to work properly.
- **Overriding Lifecycle Methods**: The `OnNavigatedTo` and `OnNavigatedFrom` methods must be overridden in each view model to define custom navigation behavior. The base class provides default implementations, but your view models should implement specific logic.
- **Content Passing**: The `OnNavigatedFrom` method prepares data (in a `Dictionary<string, object>`) that can be passed to the next view, and the `OnNavigatedTo` method processes this data when the view is navigated to.

This setup enables the **content passing** mechanism and ensures the proper lifecycle handling of views in your navigation system.

---

## 💡 Key Concepts

| Concept                           | Description                                                        |
| --------------------------------- | ------------------------------------------------------------------ |
| **NavigationRegion**              | Defines a content area that can display different views            |
| **NavigationWaypoint**            | Attached property for buttons/controls to trigger navigation       |
| **Region Names**                  | String identifiers for different navigation regions                |
| **Startup ViewModel**             | The initial view(model) displayed when the application loads       |
| **OnNavigatedTo/OnNavigatedFrom** | Lifecycle hooks for passing data between regions during navigation |

---

### 📋 Requirements

- ✅ Register all Views and ViewModels with your DI container
- ✅ **One `StartupViewModel`** must be defined for the application to launch with the correct initial view
- ✅ Use `NavigationRegion.RegionName` to mark content areas
- ✅ Use `NavigationRegion.StartupViewModel` for default view on startup (must have one)
- ✅ Use `NavigationWaypoint.Region` to tell where the button will navigate to
- ✅ Use `NavigationWaypoint.ViewModel` to tell which view model to bind to the view
- ✅ Navigation uses `WeakReferenceMessenger` internally for loose coupling

---

## 🗺️ Roadmap

Track our progress and upcoming features:

| Status | Feature                                       | Description                                |
| ------ | --------------------------------------------- | ------------------------------------------ |
| ✅     | **Region-based content control**              | Core navigation system with named regions  |
| ✅     | **Convention-based ViewModel ↔ View mapping** | Automatic view resolution from view models |
| ✅     | **Content passing**                           | Pass data between views during navigation  |
| 🔜     | **Navigation history**                        | Back/forward navigation with history stack |
| ✅     | **Lifecycle events**                          | `OnNavigatedTo`, `OnNavigatedFrom` hooks   |
| 🔜     | **Animated transitions**                      | Smooth animations between view changes     |

> 💡 **Have a feature request?** [Open an issue](https://github.com/zhivko-kocev/marauders-map/issues) and let us know what you'd like to see!

---

## 📁 Suggested Project Structure

#### **Option 1: Views and ViewModels in the Same Folder**

If you prefer to keep things simple and in a single folder, you can organize your project like this:

```
YourApp/
├── 📁 Views/
│   ├── 🖼️ HomeView.xaml
│   ├── 🧠 HomeViewModel.cs
│   ├── 🖼️ SettingsView.xaml
│   ├── 🧠 SettingsViewModel.cs
│   └── 🖼️ NestedFirstView.xaml
│   └── 🧠 NestedFirstViewModel.cs
├── 📄 App.xaml.cs
├── 🪟 MainWindow.xaml
└── 📄 ...
```

#### **Option 2: Views and ViewModels Grouped by Feature (e.g., `Songs` Folder)**

Alternatively, if you prefer to organize views and view models by feature (e.g., grouping them into folders), you can structure your project like this:

```
YourApp/
├── 📁 Songs/
│   ├── 🖼️ SongsView.xaml
│   ├── 🧠 SongsViewModel.cs
├── 📁 Settings/
│   ├── 🖼️ SettingsView.xaml
│   ├── 🧠 SettingsViewModel.cs
├── 📁 NestedViews/
│   ├── 🖼️ NestedFirstView.xaml
│   ├── 🧠 NestedFirstViewModel.cs
│   ├── 🖼️ NestedSecondView.xaml
│   └── 🧠 NestedSecondViewModel.cs
├── 📄 App.xaml.cs
├── 🪟 MainWindow.xaml
└── 📄 ...
```

In this structure:

- **Songs** folder contains both `SongsView.xaml` and `SongsViewModel.cs`, making it easier to manage related components.
- This approach scales well for larger applications with multiple features or modules.

---

## 🧪 Try It Out

Want to see it in action? Check out our example project:

[![Test Project](https://img.shields.io/badge/🚀_Try_Demo-blue?style=for-the-badge)](https://github.com/zhivko-kocev/marauders-test-project)

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
