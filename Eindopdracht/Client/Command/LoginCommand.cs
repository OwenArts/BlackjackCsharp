using System;
using System.Threading.Tasks;
using Client.Commands;
using Client.ViewModel;
using Common;

namespace Client.Command;

public class LoginCommand : BaseCommand
{
    private readonly LoginWindowViewModel _loginWindowViewModel;
    private readonly NavigationService<ClientViewModel> _navigationService;

    private readonly Log _log = new(typeof(LoginCommand));

    public LoginCommand(LoginWindowViewModel viewModel, NavigationService<ClientViewModel> navigationService)
    {
        _loginWindowViewModel = viewModel;
        _navigationService = navigationService;
    }

    public override async void Execute(object? parameter)
    { 
        await ExecuteAsync();
    }

    /// <summary>
    /// It connects to the server, sends the login credentials, waits for the server to respond, and then navigates to the
    /// next window
    /// </summary>
    public override async Task ExecuteAsync()
    {
        _log.Debug(
            $"Login button has been pressed at {System.DateTime.Now} \r\nValues are: " +
            $"{_loginWindowViewModel.Username} and {_loginWindowViewModel.SecureStringToString(_loginWindowViewModel.SecurePassword)}");
            
        await _loginWindowViewModel.Client.MakeConnectionAsync("localhost");

        if (!_loginWindowViewModel.Client.LoggedIn)
        {
            _loginWindowViewModel.Client.Username = _loginWindowViewModel.Username;
            _loginWindowViewModel.Client.Password =
                _loginWindowViewModel.SecureStringToString(_loginWindowViewModel.SecurePassword);

            try
            {
                await _loginWindowViewModel.Client.AskForLoginAsync();
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Could not start new Thread asking to login");
                throw;
            }
        }
    }
}