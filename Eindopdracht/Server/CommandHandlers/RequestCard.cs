using Newtonsoft.Json.Linq;

namespace Server.CommandHandlers;

public class RequestCard : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        parent.GiveCard(parent.Parent.Dealer.Deck.GetRandomCard());
    }
}