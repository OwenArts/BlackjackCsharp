using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Input;
using MvvmHelpers;
using Client.Commands;
using Path = System.IO.Path;

namespace Client.ViewModel;

public class LoginWindowViewModel : ObservableObject
{
    public Client_ Client;
    public ICommand LogIn { get; }

    private string _username;
    private SecureString _password;
    private readonly string _imagePath;

    /* This is the constructor of the LoginWindowViewModel. It creates a new Client and a new LoginCommand. */
    public LoginWindowViewModel(NavigationStore navigationStore)
    {
        Client = new();
        _imagePath = Path.Combine(Environment.CurrentDirectory, "Image", "Background.png");
        LogIn = new LoginCommand(this, 
            new NavigationService<ClientViewModel>(navigationStore, 
            () => new ClientViewModel(Client)));
    }

    public string Username
    {
        get => _username;
        set => _username = value;
    }

    public SecureString SecurePassword
    {
        get => _password;
        set => _password = value;
    }

    public string ImageSource
    {
        get => _imagePath;
    }
    
    /// <summary>
    /// "Convert a SecureString to a string by copying the SecureString to unmanaged memory, then copying the unmanaged
    /// memory to a managed string, then zeroing out the unmanaged memory."
    /// 
    /// The first thing to notice is that the function returns a string.  This is the string that you want to use in your
    /// code.  The SecureString is only used to get the string.  The SecureString is not used in the code that uses the
    /// string
    /// </summary>
    /// <param name="value">The SecureString object that you want to convert to a string.</param>
    /// <returns>
    /// A string
    /// </returns>
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