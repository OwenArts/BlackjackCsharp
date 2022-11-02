using Newtonsoft.Json.Linq;
using static Common.Util;

namespace Server.CommandHandlers;

public class Disconnect : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        if(packet["data"]!["destruct"]!.ToObject<bool>())
            parent.SelfDestruct(true);
        
        if (!parent.IsPlaying) return;
        foreach (var client in parent.Parent.Clients.Where(client => client.IsPlaying))
        {
            client.SendMessageAsync(SendReplacedObject("user", parent.Username, 1, "Response\\disconnected.json")!);
        }
        
        parent.NotifyNextClient();
    }
}