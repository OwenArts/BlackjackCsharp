using System;
using MvvmHelpers;

namespace Client;

public class NavigationStore
{
    public event Action CurrentViewModelChanged;
    private ObservableObject _currentViewModel;

    public Client_ Client { get; }

    public NavigationStore()
    {
        Client = new();
    }

    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}