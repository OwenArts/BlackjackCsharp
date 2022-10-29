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

        Assert.AreEqual(true, client.Connected, "Failed to connect to server");
    }
}