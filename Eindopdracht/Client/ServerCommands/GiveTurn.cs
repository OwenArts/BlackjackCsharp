using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class GiveTurn : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        throw new System.NotImplementedException();
    }
}