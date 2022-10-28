using System.Threading.Tasks;
using Client.Commands;
using Common;

namespace Client.Command;

public class StopCommand : BaseCommand
{
    private readonly Client_ _client;

    private readonly Log _log = new(typeof(JoinGameCommand));

    public StopCommand(Client_ client)
    {
        _client = client;
    }

    public override void Execute(object? parameter)
    {
        _client.Stop();
    }

    public override async Task ExecuteAsync()
    {
    }
}