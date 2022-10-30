using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class GoBust : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        if (!parent.IsPlaying) return;
        var viewModel = (ClientViewModel)parent.ViewModel;
        viewModel.MiddleMessage = "Helaas! U bent over 21 gegaan";
        viewModel.HasTurn = false;
    }
}