using System.Net.Sockets;
using Server;

namespace ServerTest;

[TestClass]
public class ServerTest
{
    private ServerSocket _serverSocket;
    private TcpClient _client;
    private NetworkStream _stream;
    private readonly byte[] _buffer = new byte[1024];


    [TestMethod]
    public async void TestServerConnection()
    {
        _serverSocket = new ServerSocket();
        _client = new TcpClient();

        await _client.ConnectAsync("localhost", 7492);
        _stream = _client.GetStream();
        _stream.BeginRead(_buffer, 0, 1024, null, null);

        Assert.AreEqual(true, _client.Connected, "Failed to connect to server");
    }
    
    // [TestMethod]
    // public async void TestServerOnRead()
    // {
    //     Assert.AreEqual(true, _client.Connected, "Failed to connect to server");
    // }
}