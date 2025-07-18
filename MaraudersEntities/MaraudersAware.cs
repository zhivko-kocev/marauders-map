using CommunityToolkit.Mvvm.ComponentModel;

namespace Kocew.WPF.MaraudersMap.MaraudersEntities;

public abstract class MaraudersAware: ObservableRecipient, IMaraudersAware
{
    public virtual void OnNavigatedTo(Type sender, Dictionary<string, object> content){}

    public virtual Dictionary<string, object> OnNavigatedFrom()
    {
        return [];
    }
}