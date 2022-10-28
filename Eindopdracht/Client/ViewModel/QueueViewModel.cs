using MvvmHelpers;

namespace Client.ViewModel;

public class QueueViewModel : ObservableObject
{
    private Client_ _client;
    private NavigationStore _navStore;

    private string _username;

    public QueueViewModel(Client_ client, NavigationStore navigationStore)
    {
        _client = client;
        _client.ViewModel = this;
        _navStore = navigationStore;
    }
    
    public string CurrentUserName
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }
}