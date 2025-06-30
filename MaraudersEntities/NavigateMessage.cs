namespace Kocew.WPF.MaraudersMap.MaraudersEntities;

/// <summary>
/// Message used to trigger navigation to a view model within a specific region.
/// This message is typically sent to a navigation service to navigate to a specific 
/// view model in a defined region (host control) within the application.
/// </summary>
/// <param name="ViewModelType">
/// The type of the ViewModel to navigate to. This specifies which view model
/// the navigation system should instantiate or display in the specified region.
/// </param>
/// <param name="Region">
/// The name of the region (host control) where the navigation should occur.
/// This typically corresponds to a specific part of the UI where the view model
/// will be displayed.
/// </param>
/// <param name="Parameter">
/// An optional parameter that can be passed to the ViewModel during navigation.
/// This can be used to pass any necessary data to the ViewModel, such as 
/// initialization parameters or data required for the view.
/// </param>
public sealed record NavigateMessage(Type ViewModelType, string Region, object? Parameter = null);