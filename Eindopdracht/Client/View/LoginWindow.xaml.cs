using System.Windows;
using System.Windows.Controls;

namespace Client.View;

public partial class LoginWindow : UserControl
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.DataContext != null)
        {
            ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
        }
    }
}