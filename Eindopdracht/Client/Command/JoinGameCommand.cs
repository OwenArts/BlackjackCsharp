using System.Threading.Tasks;
using Client.Commands;
using Client.ViewModel;
using Common;

namespace Client.Command;

public class JoinGameCommand : BaseCommand
{
    private readonly QueueViewModel _loginWindowViewModel;
    private readonly NavigationService<ClientViewModel> _navigationService;

    private readonly Log _log = new(typeof(JoinGameCommand));

    public JoinGameCommand(QueueViewModel viewModel, NavigationService<ClientViewModel> navigationService)
    {
        _loginWindowViewModel = viewModel;
        _navigationService = navigationService;
    }

    public override void Execute(object? parameter)
    {
        ExecuteAsync();
    }

    public override async Task ExecuteAsync()
    {
        _navigationService.Navigate();
    }
}