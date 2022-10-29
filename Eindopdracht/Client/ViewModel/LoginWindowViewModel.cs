using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Input;
using Client.Command;
using MvvmHelpers;
using Client.Commands;
using Path = System.IO.Path;

namespace Client.ViewModel;

public class LoginWindowViewModel : ObservableObject
{
    public Client_ Client;
    public ICommand LogIn { get; }
    public ICommand CreateAccount { get; }

    private string _username;
    private SecureString _password;

    public LoginWindowViewModel(NavigationStore navigationStore)
    {
        Client = navigationStore.Client;
        Client.addViewModel(this);
        LogIn = new LoginCommand(this,
            new NavigationService<QueueViewModel>(navigationStore,
                () => new QueueViewModel(Client, navigationStore)));
        CreateAccount = new CreateAccountCommand(this,
            new NavigationService<QueueViewModel>(navigationStore,
                () => new QueueViewModel(Client, navigationStore)));
    }

    public string Username
    {
        get
        {
            if (_username != null && _username.Length > 0)
                return _username;
            else
                return "";
        }
        set => _username = value;
    }

    public SecureString SecurePassword
    {
        get
        {
            if (_password != null)
                return _password;
            else
                return null;
        }
        set => _password = value;
    }

    public string SecureStringToString(SecureString value)
    {
        IntPtr valuePtr = IntPtr.Zero;
        try
        {
            valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
            return Marshal.PtrToStringUni(valuePtr);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
        }
    }
}