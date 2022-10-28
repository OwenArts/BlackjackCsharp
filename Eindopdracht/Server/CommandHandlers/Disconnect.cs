using Newtonsoft.Json.Linq;

namespace Server.CommandHandlers;

public class Disconnect : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        if(packet["data"]!["destruct"]!.ToObject<bool>())
            parent.SelfDestruct(true);
        
        parent.NotifyNextClient();
    }
}