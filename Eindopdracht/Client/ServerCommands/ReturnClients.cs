using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class ReturnClients : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        var playingClients = packet["data"]!["clients"]!.ToObject<string[]>()!;
        parent.OtherPlayers = playingClients;
    }
}