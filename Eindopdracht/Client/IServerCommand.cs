using Newtonsoft.Json.Linq;

namespace Client;

public interface IServerCommand
{
    void OnCommandReceived(JObject packet, Client_ parent);
}