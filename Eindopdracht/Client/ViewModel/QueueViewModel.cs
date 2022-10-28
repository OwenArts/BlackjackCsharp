using System.Threading.Tasks;
using System.Windows.Input;
using Client.Command;
using MvvmHelpers;

namespace Client.ViewModel;

public class QueueViewModel : ObservableObject
{
    private Client_ _client;
    private NavigationStore _navStore;

    private string _username;

    public ICommand JoinGame { get; }

    public QueueViewModel(Client_ client, NavigationStore navigationStore)
    {
        _client = client;
        _client.addViewModel(this);
        _navStore = navigationStore;

        JoinGame = new JoinGameCommand(this, 
            new NavigationService<ClientViewModel>(navigationStore, 
                () => new ClientViewModel(client)));
    }
}