using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class GameStarted : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        if (!parent.IsPlaying) return;
        var viewModel = (ClientViewModel)parent.ViewModel;
        viewModel.GameStarted = true;
        viewModel.MiddleMessage = "";
    }
}