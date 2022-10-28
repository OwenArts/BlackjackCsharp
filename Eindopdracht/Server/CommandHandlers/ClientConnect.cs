using Newtonsoft.Json.Linq;
using static Common.Util;

namespace Server.CommandHandlers;

public class ClientConnect : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        var username = packet["data"]!["username"]!.ToObject<string>()!;
        var password = packet["data"]!["password"]!.ToObject<string>()!;
        var allUsers = GetJson("storage\\accounts.json")["accounts"]!.ToObject<string[][]>()!;
        var clients = new List<string>();

        foreach (var client in parent.Parent.Clients)
        {
            if(!string.IsNullOrEmpty(client.Username)) clients.Add(client.Username);
            parent.SendMessage(SendReplacedObject("status", 1, 1, SendReplacedObject(
                "message", "Er is al een account ingelogd met deze gebruikersnaam.", 1, "packets\\clientconnected.json"
            )!)!);
            parent.SelfDestruct(true);
            return;
        }

        var exists = allUsers.Where(user => user[0] == username).Any(user => user[1] == password);

        if (!exists)
        {
            parent.SendMessage(SendReplacedObject("status", 3, 1, SendReplacedObject(
                "message", "Dit account bestaat niet.", 1, "packets\\clientconnected.json"
            )!)!);
            parent.SelfDestruct(true);
            return;
        }

        if (parent.Parent.Clients.Count >= 4)
        {
            parent.SendMessage(SendReplacedObject("status", 2, 1, SendReplacedObject(
                "message", "Er zijn te veel mensen in de applicatie. U staat nu in de wachtrij.", 1, "packets\\clientconnected.json"
            )!)!);
            parent.SelfDestruct(true);
            return;
        }

        parent.Username = username;
        
        parent.SendMessage(SendReplacedObject("status", 0, 1, SendReplacedObject(
            "clients", clients.ToArray(), 1, "packets\\clientconnected.json"
        )!)!);
    }
}