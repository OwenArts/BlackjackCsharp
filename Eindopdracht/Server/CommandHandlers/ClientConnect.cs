using Common;
using Newtonsoft.Json.Linq;
using static Common.Util;

namespace Server.CommandHandlers;

public class ClientConnect : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        var username = packet["data"]!["username"]!.ToObject<string>()!;
        var password = packet["data"]!["password"]!.ToObject<string>()!;
        var allUsers = GetJson("Storage\\accounts.json")["accounts"]!.ToObject<string[][]>()!;

        if (parent.Parent.Clients.Any(client => client.Username == username))
        {
            parent.SendMessage(SendReplacedObject("status", 1, 1, "Response\\clientconnected.json")!);
            parent.SelfDestruct(true);
            return;
        }

        var exists = allUsers.Where(user => user[0] == username).Any(user => user[1] == password);

        if (!exists)
        {
            parent.SendMessage(SendReplacedObject("status", 3, 1, "Response\\clientconnected.json")!);
            parent.SelfDestruct(true);
            return;
        }

        if (parent.Parent.Clients.Count > 4)
        {
            parent.SendMessage(SendReplacedObject("status", 2, 1, "Response\\clientconnected.json")!);
            parent.SelfDestruct(true);
            return;
        }

        parent.Username = username;
        Log.Send().Information("Login Successful");
        parent.SendMessage(SendReplacedObject("status", 0, 1, "Response\\clientconnected.json")!);
    }
}