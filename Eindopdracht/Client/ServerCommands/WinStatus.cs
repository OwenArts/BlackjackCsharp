using System.Threading;
using System.Threading.Tasks;
using Client.ViewModel;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class WinStatus : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        var status = packet["data"]!["win"]!.ToObject<int>();
        var balance = packet["data"]!["balance"]!.ToObject<int>();
        var viewModel = (ClientViewModel)parent.ViewModel;
        viewModel.Money = balance;
        viewModel.MiddleMessage = status switch
        {
            0 => "U hebt verloren",
            1 => "U hebt gewonnen!",
            2 => "U hebt gelijkgespeeld",
            _ => viewModel.MiddleMessage
        };
        Thread.Sleep(4000);
        viewModel.GameStarted = false;
        viewModel.Reset();
    }
}