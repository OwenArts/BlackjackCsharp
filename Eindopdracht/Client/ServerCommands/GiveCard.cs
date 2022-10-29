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
        string imagePath = "";
        switch (suite)
        {
            case 0:
                imagePath = "h";
                break;
            case 1:
                imagePath = "s";
                break;
            case 2:
                imagePath = "d";
                break;
            case 3:
                imagePath = "c";
                break;
        }

        imagePath += packet["data"]!["piece"]!.ToObject<int>().ToString();
        imagePath = Path.Combine(
            Environment.CurrentDirectory.Substring(0,
                Environment.CurrentDirectory.LastIndexOf("bin", StringComparison.Ordinal)), "Image", "Cards",
            $"{imagePath}.png");
        Log.Send().Debug(imagePath);

        //todo, send card to the right observableCollection
        _viewModel!.DealerCard.Add(imagePath);
        _viewModel!.UpdateProperty();
    }
}