using Newtonsoft.Json.Linq;

namespace Client;

public interface IServerCommand
{
    void OnCommandReceivedAsync(JObject packet, Client_ parent);
}