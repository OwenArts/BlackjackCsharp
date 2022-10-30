using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class StopGame : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        if (!parent.IsPlaying) return;
        var viewModel = (ClientViewModel)parent.ViewModel;
        viewModel.Reset();
        viewModel.MiddleMessage = "";
    }
}