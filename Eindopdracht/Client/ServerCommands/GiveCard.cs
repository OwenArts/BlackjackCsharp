using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Shapes;
using Client.ViewModel;
using Common;
using Newtonsoft.Json.Linq;
using Path = System.IO.Path;

namespace Client.ServerCommands;

public class GiveCard : IServerCommand
{
    private ClientViewModel _viewModel;

    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        try
        {
            _viewModel = (ClientViewModel)parent.ViewModel;
        }
        catch (Exception e)
        {
            Log.Send().Error(e, "Could not parse parent.viewModel into ClientViewModel");
            throw;
        }

        var suite = packet["data"]!["suite"]!.ToObject<int>();
        var imagePath = suite switch
        {
            0 => "h",
            1 => "s",
            2 => "d",
            3 => "c",
            _ => ""
        };

        imagePath += packet["data"]!["piece"]!.ToObject<int>().ToString();
        imagePath = Path.Combine(
            Environment.CurrentDirectory.Substring(0,
                Environment.CurrentDirectory.LastIndexOf("bin", StringComparison.Ordinal)), "Image", "Cards",
            $"{imagePath}.png");
        Log.Send().Debug(imagePath);

        var user = packet["data"]!["user"]!.ToObject<string>()!;
        var value = packet["data"]!["value"]!.ToObject<int>()!;
        _viewModel.UpdateCards(user, imagePath, value);
    }
}