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
                await Task.Delay(1000);
                parent.ExitQueue();
                return;
            case 1:
                MessageBox.Show("Er is al een account met deze gebruikersnaam ingelogd");
                break;
            case 2:
                parent.LoggedIn = true;
                return;
            case 3:
                MessageBox.Show("Er is geen account gevonden met dit wachtwoord en gebruikersnaam");
                break;
        }
        parent.SelfDestruct();
    }
}