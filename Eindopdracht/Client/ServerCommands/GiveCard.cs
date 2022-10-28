using Common;
using Newtonsoft.Json.Linq;

namespace Client.ServerCommands;

public class GiveCard : IServerCommand
{
    public void OnCommandReceivedAsync(JObject packet, Client_ parent)
    {
        var suite = packet["data"]!["suite"]!.ToObject<int>();
        string imageName = "";
        switch (suite)
        {
            case 0 :
                imageName = "h";
                break;
            case 1 :
                imageName = "s";
                break;
            case 2 :
                imageName = "d";
                break;
            case 3 :
                imageName = "c";
                break;
        }
        imageName += packet["data"]!["piece"]!.ToObject<int>().ToString();
        Log.Send().Debug(imageName);
        
        
    }
}