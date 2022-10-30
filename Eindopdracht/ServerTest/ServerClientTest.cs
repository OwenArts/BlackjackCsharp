using System.Net.Sockets;
using Common;
using Server;

namespace ServerTest;

[TestClass]
public class ServerClientTest
{
    [TestMethod]
    public void ServerClientTestCalculateWin()
    {
        var server = new ServerSocket();
        var client = new TcpClient();
        int winCode = -1;

        client.Connect("localhost", 7492);

        Assert.AreEqual(true, client.Connected, "Failed to connect to server.");

        Thread.Sleep(500);
        
        if (server.Clients.Count > 0)
            winCode = server.Clients[0].CalculateWin(20, 21);

        Assert.AreEqual(1, winCode, "Failed to return correct win code.");
    }
    
    [TestMethod]
    public void ServerClientTestPlaceBet()
    {
        var server = new ServerSocket();
        var client = new TcpClient();
        int bet = 500;
        
        client.Connect("localhost", 7492);

        Assert.AreEqual(true, client.Connected, "Failed to connect to server.");

        Thread.Sleep(500);
        
        if (server.Clients.Count > 0)
            bet = server.Clients[0].PlaceBet(bet);

        Assert.AreEqual(500, bet, "Failed to place bet.");
    }
    
    [TestMethod]
    public void ServerClientTestDoubleDown()
    {
        var server = new ServerSocket();
        var client = new TcpClient();
        int bet = 500;
        
        client.Connect("localhost", 7492);

        Assert.AreEqual(true, client.Connected, "Failed to connect to server.");

        Thread.Sleep(500);
        
        if (server.Clients.Count > 0)
            bet = server.Clients[0].DoubleDown(bet);

        Assert.AreEqual(1000, bet, "Failed to double bet.");
    }
}