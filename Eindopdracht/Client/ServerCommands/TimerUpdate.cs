using System;
using Client.ViewModel;
using Common;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class TimerUpdate : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        try
        {
            var time = packet["data"]!["time"]!.ToObject<int>();
            var viewModel = (ClientViewModel)parent.ViewModel;
            viewModel.MiddleMessage = time.ToString();
        }
        catch(Exception e)
        {
            Log.Send().Error(e.Message);
        }
    }
}