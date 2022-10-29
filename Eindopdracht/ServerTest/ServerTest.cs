using System.Net.Sockets;
using Server;
using Common;

namespace ServerTest;

[TestClass]
public class ServerTest
{
    [TestMethod]
    public void TestServerConnection()
    {
        new ServerSocket();
        var client = new TcpClient();

        client.Connect("localhost", 7492);

        Task.Delay(1000);

        Assert.AreEqual(true, client.Connected, "Failed to connect to server");
    }

    [TestMethod]
    public void TestServerSelfDestruct()
    {
        var server = new ServerSocket();
        var client = new TcpClient();

        client.Connect("localhost", 7492);
        
        server.Clients[0].SelfDestruct(true);
        
        Assert.AreEqual(false, client.Connected, "Failed to self destruct client");

    }
}