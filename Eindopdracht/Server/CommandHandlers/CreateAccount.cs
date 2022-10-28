using Common;
using Newtonsoft.Json.Linq;
using static Common.Util;

namespace Server.CommandHandlers;

public class CreateAccount : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        Log.Send().Debug($"OnCommandReceived entered, {packet}");
        var username = packet["data"]!["username"]!.ToObject<string>()!;
        var password = packet["data"]!["password"]!.ToObject<string>()!;
        var oAccounts = GetJson("Storage\\accounts.json");
        var accounts = oAccounts["accounts"]!.ToObject<string[][]>()!.ToList();

        Log.Send().Critical($"{accounts.Count}");

        foreach (var acc in accounts)
        {
            Log.Send().Error($"Username, {username} : found: {acc[0]}");
            if (acc[0] == username)
            {
                try
                {
                    parent.SendMessage(SendReplacedObject("status", 1, 1, "Response\\accountcreated.json")!);
                    parent.SelfDestruct(true);
                }
                catch (Exception e)
                {
                    Log.Send().Error(e, "Could not send message");
                    throw;
                }
                return;
            }
        }


        accounts.Add(new[] { username, password });
        var jArray = new JArray();
        foreach (var account in accounts)
        {
            var array = new JArray(account.ToList());
            jArray.Add(array);
        }

        oAccounts["accounts"] = jArray;

        Log.Send().Debug($"Writing to Acounts.json: {oAccounts}");

        WriteJson(oAccounts, "Storage\\accounts.json");
        parent.SendMessage(SendReplacedObject("status", 0, 1, "Response\\accountcreated.json")!);

        /*
        if (accounts.Any(account => account[0] != username))
        {
            accounts.Add(new[] { username, password });
            var jArray = new JArray();
            foreach (var account in accounts)
            {
                var array = new JArray(account.ToList());
                jArray.Add(array);
            }

            oAccounts["accounts"] = jArray;

            Log.Send().Debug($"Writing to Acounts.json: {oAccounts}");

            WriteJson(oAccounts, "Storage\\accounts.json");
            parent.SendMessage(SendReplacedObject("status", 0, 1, "Response\\accountcreated.json")!);

            return;
        }
        */

        /*parent.SendMessage(SendReplacedObject("status", 1, 1, "Response\\accountcreated.json")!);

        parent.SelfDestruct(true);*/
    }
}