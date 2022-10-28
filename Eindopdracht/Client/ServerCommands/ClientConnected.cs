using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class ClientConnected : IServerCommand
{
    public async void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        var status = packet["data"]!["status"]!.ToObject<int>();
        switch (status)
        {
            case 0:
                parent.LoggedIn = true;
                parent.Balance = packet["data"]!["money"]!.ToObject<int>();
                await Task.Delay(1000);
                parent.ExitQueue();
                return;
            case 1:
                MessageBox.Show("Er is al een apparaat met dit account ingelogd.");
                break;
            case 2:
                parent.LoggedIn = true;
                return;
            case 3:
                MessageBox.Show("Er is geen account gevonden met dit wachtwoord en gebruikersnaam.");
                break;
            case 4:
                MessageBox.Show("De ingevoerde gebruikersnaam is al in bezit genomen.");
                break;
        }
        parent.SelfDestruct();
    }
}