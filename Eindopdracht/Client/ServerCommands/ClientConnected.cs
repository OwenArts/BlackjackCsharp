using System.Windows;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class ClientConnected : IServerCommand
{
    public void OnCommandReceived(JObject packet, Client_ parent)
    {
        var status = packet["data"]!["status"]!.ToObject<int>();
        switch (status)
        {
            case 0:
                parent.LoggedIn = true;
                parent.Queued = false;
                return;
            case 1:
                MessageBox.Show("Er is al een account met deze gebruikersnaam ingelogd");
                break;
            case 2:
                MessageBox.Show("Er zijn te veel spelers in het spel. U zit nu in de queue");
                
                parent.LoggedIn = true;
                parent.Queued = true;
                break;
            case 3:
                MessageBox.Show("Er is geen account gevonden met dit wachtwoord en gebruikersnaam");
                break;
        }
        parent.SelfDestruct();
    }
}