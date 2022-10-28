using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using Client.ServerCommands;
using Client.ViewModel;
using Common;
using Newtonsoft.Json.Linq;
using static Common.Cryptographer;
using static Common.Util;

namespace Client;

public class Client_
{
    private Dictionary<string, IServerCommand> _commands;

    private readonly Log _log = new(typeof(Client_));
    private const int Port = 7492;
    private TcpClient _tcpClient;
    private NetworkStream _stream;

    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public ClientViewModel ViewModel { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public bool LoggedIn { get; set; }

    public Client_()
    {
        _commands = new Dictionary<string, IServerCommand>();
        InitCommands();
    }

    public async Task MakeConnectionAsync(string ip)
    {
        if (_tcpClient.Connected)
            return;

        var attempts = 0;

        while (attempts < 5)
        {
            attempts++;
            
            _log.Information($"Connecting to {ip}:{Port} (attempt #{attempts})");

            try
            {
                await Connect(ip);
                _log.Information($"Connected to {ip}:{Port}");
                _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
                break;
            }
            catch (Exception ex)
            {
                _tcpClient.Close();
                _tcpClient = new TcpClient();

                if (attempts == 5)
                {
                    _log.Error(ex, $"Could not connect to {ip}:{Port}");
                    throw;
                }

                _log.Error(ex, $"Could not connect to {ip}:{Port} ... retrying");
            }
            
            await Task.Delay(1000);
        }
    }

    private async Task Connect(string ip)
    {
        _tcpClient = new TcpClient();
        await _tcpClient.ConnectAsync(ip, Port);
        _stream = _tcpClient.GetStream();
    }

    private void OnRead(IAsyncResult ar)
    {
        try
        {
            var rc = _stream.EndRead(ar);
            _totalBuffer = Concat(_totalBuffer, _buffer, rc);

            while (_totalBuffer.Length >= 4)
            {
                var data = GetDecryptedMessage(_totalBuffer);
                _totalBuffer = Array.Empty<byte>();

                if (_commands.ContainsKey(data["id"]!.ToObject<string>()!))
                    _commands[data["id"]!.ToObject<string>()!].OnCommandReceived(data, this);

                break;
            }

            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch (Exception)
        {
            Stop();
        }
    }

    private void SendData(JObject message)
    {
        var encryptedMessage = GetEncryptedMessage(message);
        try
        {
            _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
        }
        catch (Exception)
        {
            Stop();
        }
    }

    public void LogIn()
    {
        LoggedIn = true;
        MessageBox.Show("Vanaf hier verder");
    }

    public void Stop()
    {
        if (!_tcpClient.Connected) return;
        LoggedIn = false;
        SendData(GetJson("Client\\Packets\\disconnect.json"));
    }

    public void SelfDestruct()
    {
        _stream.Close(1000);
        _tcpClient.Close();
    }

    public async Task AskForLoginAsync()
    {
        throw new NotImplementedException();
    }

    private void InitCommands()
    {
        _commands.Add("client/connected", new ClientConnected());
    }
}