using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Client.ViewModel;
using Common;

namespace Client;

public class Client_
{
    private readonly Log _log = new(typeof(Client_));
    private const int Port = 7492;
    private TcpClient _tcpClient;

    public ClientViewModel ViewModel;

    public string Username {get;set;}
    public string Password {get;set;}
    public bool LoggedIn { get; set; }

    public Client_()
    {
    }

    public async Task MakeConnectionAsync(string ip, int port)
    {
        if (_tcpClient.Connected)
            return;

        var attempts = 0;

        while (attempts < 5)
        {
            attempts++;
            
            _log.Information($"Connecting to {ip}:{port} (attempt #{attempts})");

            try
            {
                // await Socket.ConnectAsync(IPAddress.Parse(ip), port);
                _log.Information($"Connected to {ip}:{port}");
                // Read();
                break;
            }
            catch (Exception ex)
            {
                _tcpClient.Close();
                _tcpClient = new TcpClient();

                if (attempts == 5)
                {
                    _log.Error(ex, $"Could not connect to {ip}:{port}");
                    throw;
                }

                _log.Error(ex, $"Could not connect to {ip}:{port} ... retrying");
            }
            
            await Task.Delay(1000);
        }
    }

    public async Task AskForLoginAsync()
    {
        throw new NotImplementedException();
    }
}