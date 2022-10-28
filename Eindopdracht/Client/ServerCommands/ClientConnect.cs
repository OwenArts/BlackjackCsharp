using System.Windows;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class ClientConnect : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        MessageBox.Show("U kunt dit niet betalen");
    }
}