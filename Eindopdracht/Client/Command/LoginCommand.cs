using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Client.ViewModel;
using Common;

namespace Client.Commands;

public class LoginCommand : BaseCommand
{
    private readonly NavigationStore _navigationStore;
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
        await _loginWindowViewModel.Client.MakeConnectionAsync("localhost");

        if (!_loginWindowViewModel.Client.LoggedIn)
        {
            _loginWindowViewModel.Client.Username = _loginWindowViewModel.Username;
            _loginWindowViewModel.Client.Password = _loginWindowViewModel.SecureStringToString(_loginWindowViewModel.SecurePassword);
            
            try
            {
                new Thread(async () =>
                {
                    await _loginWindowViewModel.Client.AskForLoginAsync();
                }).Start();
            } catch (Exception exception)
            {
                _log.Error(exception, "Could not start new Thread asking to login");
                throw;
            }

            await Task.Delay(1000);
            
            if (_loginWindowViewModel.Client.LoggedIn)
            {
                // await _loginWindowViewModel.Client.RequestPatientDataAsync();
                await Task.Delay(1000);

                _navigationService.Navigate();
            }
        }
    }
}