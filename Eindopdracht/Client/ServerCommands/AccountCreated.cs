using System.Threading.Tasks;
using System.Windows;
using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class AccountCreated : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        var status = packet["data"]!["status"]!.ToObject<int>();
        switch (status)
        {
            case 0:
                if (((LoginWindowViewModel)parent.ViewModel).LogIn.CanExecute(null))
                    ((LoginWindowViewModel)parent.ViewModel).LogIn.Execute(null);
                return;
            case 1:
                MessageBox.Show("De ingevoerde gebruikersnaam is al in bezit genomen.");
                break;
        }

        parent.SelfDestruct();
    }
}