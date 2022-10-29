using System.Net.Sockets;
using Server;

namespace ServerTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestServerConnection()
    {
        new ServerSocket();
        var client = new TcpClient();
        
        client.Connect("localhost", 7492);
        
        Assert.AreEqual(true, client.Connected, "Failed to connect to server");
    }
    
    [TestMethod]
    public void TestServerOnRead()
    {
        new ServerSocket();
        var client = new TcpClient();
        
        client.Connect("localhost", 7492);
        
        Assert.AreEqual(true, client.Connected, "Failed to connect to server");
    }
}