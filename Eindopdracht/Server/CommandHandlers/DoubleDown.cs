using Newtonsoft.Json.Linq;

namespace Server.CommandHandlers;

public class DoubleDown : ICommandAction
{
    public void OnCommandReceived(JObject packet, ServerClient parent)
    {
        parent.DoubleDown();
    }
}