using Newtonsoft.Json.Linq;

namespace Server;

public interface ICommandAction
{
    void OnCommandReceived(JObject packet, ServerClient parent);
}