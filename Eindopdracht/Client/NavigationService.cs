using System;
using MvvmHelpers;

namespace Client;

public class NavigationService<TObservableObject>
    where TObservableObject : ObservableObject
{
    private readonly NavigationStore _navigationStore;

    private readonly Func<TObservableObject> _createViewModel;

    public NavigationService(NavigationStore navigationStore, Func<TObservableObject> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate()
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}