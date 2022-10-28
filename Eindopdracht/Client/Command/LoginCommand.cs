using System;
using System.Threading.Tasks;
using Client.Commands;
using Client.ViewModel;
using Common;

namespace Client.Command;

public class LoginCommand : BaseCommand
{
    private readonly LoginWindowViewModel _loginWindowViewModel;
    private readonly NavigationService<QueueViewModel> _navigationService;

    private readonly Log _log = new(typeof(LoginCommand));

    public LoginCommand(LoginWindowViewModel viewModel, NavigationService<QueueViewModel> navigationService)
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

    /// <summary>
    /// It connects to the server, sends the login credentials, waits for the server to respond, and then navigates to the
    /// next window
    /// </summary>
    public override async Task ExecuteAsync()
    {
        await _loginWindowViewModel.Client.MakeConnectionAsync("localhost");

        if (!_loginWindowViewModel.Client.LoggedIn)
        {
            _loginWindowViewModel.Client.Username = _loginWindowViewModel.Username;
            _loginWindowViewModel.Client.Password =
                _loginWindowViewModel.SecureStringToString(_loginWindowViewModel.SecurePassword);

            try
            {
                await _loginWindowViewModel.Client.CreateAccountAsync("localhost");
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Could not start new Thread asking to login");
                throw;
            }

            await Task.Delay(500);

            if (_loginWindowViewModel.Client.LoggedIn)
            {
                _navigationService.Navigate();
            }
        }
    }
}