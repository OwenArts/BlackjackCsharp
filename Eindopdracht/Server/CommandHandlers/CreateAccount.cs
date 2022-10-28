using Newtonsoft.Json.Linq;
using static Common.Util;

namespace Server.CommandHandlers;

public class CreateAccount : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        var username = packet["data"]!["username"]!.ToObject<string>()!;
        var password = packet["data"]!["password"]!.ToObject<string>()!;
        var oAccounts = GetJson("Storage\\accounts.json");
        var accounts = oAccounts["accounts"]!.ToObject<string[][]>()!.ToList();

        if (accounts.Any(account => account[0] == username))
        {
            parent.SendMessage(SendReplacedObject("status", 0, 1, "Response\\accountcreated.json")!);
            return;
        }

        accounts.Add(new[]{username,password});
        var jArray = new JArray();
        foreach (var account in accounts)
        {
            var array = new JArray(account.ToList());
            jArray.Add(array);
        }

        oAccounts["accounts"] = jArray;

        WriteJson(oAccounts, "Storage\\accounts.json");
        parent.SendMessage(SendReplacedObject("status", 1, 1, "Response\\accountcreated.json")!);
        
        parent.SelfDestruct(true);
    }
}