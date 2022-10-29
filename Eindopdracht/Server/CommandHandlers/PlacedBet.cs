using Newtonsoft.Json.Linq;

namespace Server.CommandHandlers;

public class PlacedBet : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        var bet = packet["data"]!["bet"]!.ToObject<int>();
        parent.PlaceBet(bet);
    }
}