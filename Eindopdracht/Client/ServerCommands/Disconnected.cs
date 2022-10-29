using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class Disconnected : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        var disconnectedClient = packet["data"]!["user"]!.ToObject<string>()!;
        var viewModel = (ClientViewModel)parent.ViewModel;
        if (disconnectedClient == viewModel.Player1.Name) viewModel.Player1.Name = "";
        else if (disconnectedClient == viewModel.Player2.Name) viewModel.Player2.Name = "";
        else if (disconnectedClient == viewModel.Player3.Name) viewModel.Player3.Name = "";
    }
}