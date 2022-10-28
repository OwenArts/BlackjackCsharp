using Newtonsoft.Json.Linq;

namespace Server.CommandHandlers;

public class CallDeck : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        parent.Parent.Dealer.GiveTurn();
    }
}