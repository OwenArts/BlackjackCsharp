using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class GiveTurn : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        var viewModel = (ClientViewModel)parent.ViewModel;
        viewModel.MiddleMessage = "Het is uw beurt!";
        viewModel.HasTurn = true;
    }
}