namespace Kocew.WPF.MaraudersMap.MaraudersEntities;

public interface IMaraudersAware
{
    void OnNavigatedTo(Type sender, Dictionary<string, object> content);
    
    Dictionary<string, object> OnNavigatedFrom();
}