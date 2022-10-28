using System.Net;
using System.Net.Sockets;

namespace Server;

public class ServerSocket
{

    public List<ServerClient> Clients { get; }
    private readonly TcpListener _listener;
    private const int Port = 7492;
    
    public ServerSocket()
    {
        Clients = new List<ServerClient>();
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
}