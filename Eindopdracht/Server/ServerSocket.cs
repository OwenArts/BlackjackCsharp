using System.Net;
using System.Net.Sockets;
using Common;
using static Common.Util;

namespace Server;

public class ServerSocket
{
    public List<ServerClient> Clients { get; }
    private readonly TcpListener _listener;
    private const int Port = 7492;
    public Dealer Dealer { get; }

    public ServerSocket()
    {
        Clients = new List<ServerClient>();
        Dealer = new Dealer(this);
        _listener = new TcpListener(IPAddress.Any, Port);
        _listener.Start();
        _listener.BeginAcceptTcpClient(OnConnect, null);
    }

    private void OnConnect(IAsyncResult ar)
    {
        var tcp = _listener.EndAcceptTcpClient(ar);
        ServerClient client = new(tcp, this);
        new Thread(client.Start).Start();
        Clients.Add(client);
        _listener.BeginAcceptTcpClient(OnConnect, this);
    }

    public void GiveDealerCard(int piece, int suite, int value)
    {
        foreach (var client in Clients)
        {
            client.SendMessage(SendReplacedObject("user", "Dealer", 1, SendReplacedObject(
                "piece", piece, 1, SendReplacedObject(
                    "suite", suite, 1, SendReplacedObject(
                        "value", value, 1, "Response\\givecard.json"
                    )
                )
            ))!);
        }
    }

    public void CalculateWinners()
    {
        foreach (var player in Dealer.PlayingClients)
        {
            player.CalculateWin(Dealer.TotalValue);
        }
    }
}