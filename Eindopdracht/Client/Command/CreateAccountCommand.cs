using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Commands;
using Client.ViewModel;
using Common;

namespace Client.Command;

public class CreateAccountCommand : BaseCommand
{
    private readonly LoginWindowViewModel _loginWindowViewModel;
    private readonly NavigationService<QueueViewModel> _navigationService;
    private readonly Log _log = new(typeof(CreateAccountCommand));

    public CreateAccountCommand(LoginWindowViewModel viewModel,
        NavigationService<QueueViewModel> navigationService)
    {
        _loginWindowViewModel = viewModel;
        _navigationService = navigationService;
    }

    public override async void Execute(object? parameter)
    {
        if (_loginWindowViewModel.Username.Length > 0 &&
            _loginWindowViewModel.SecureStringToString(_loginWindowViewModel.SecurePassword).Length > 0)
            await ExecuteAsync();
    }

    public override async Task ExecuteAsync()
    {
        _loginWindowViewModel.Client.Username = _loginWindowViewModel.Username;
        _loginWindowViewModel.Client.Password =
            _loginWindowViewModel.SecureStringToString(_loginWindowViewModel.SecurePassword);

        _loginWindowViewModel.Client.CreateAccountAsync();
    }
}