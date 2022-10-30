using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class ClientConnect : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        if (!parent.IsPlaying) return;
        var connectedClient = packet["data"]!["user"]!.ToObject<string>()!;
        var viewModel = (ClientViewModel)parent.ViewModel;
        if (viewModel.Player1.Name == "") viewModel.Player1.Name = connectedClient;
        else if (viewModel.Player2.Name == "") viewModel.Player2.Name = connectedClient;
        else if (viewModel.Player3.Name == "") viewModel.Player3.Name = connectedClient;
    }
}