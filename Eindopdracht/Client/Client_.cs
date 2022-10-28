using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using Client.ServerCommands;
using Client.ViewModel;
using Common;
using MvvmHelpers;
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

    public ObservableObject ViewModel { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public bool LoggedIn { get; set; }

    public Client_()
    {
        _commands = new Dictionary<string, IServerCommand>();
        InitCommands();
        _tcpClient = new TcpClient();
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
                    _commands[data["id"]!.ToObject<string>()!].OnCommandReceivedAsync(data, this);

                break;
            }

            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch (Exception e)
        {
            _log.Error(e, "OnRead() err");
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
        catch (Exception e)
        {
            _log.Error(e, "SendData() err");
            Stop();
        }
    }

    public void Stop()
    {
        if (!_tcpClient.Connected) return;
        LoggedIn = false;
        _log.Critical("Stop()");
        SendData(GetJson("Requests\\disconnect.json"));
    }

    public void SelfDestruct()
    {
        _stream.Close(1000);
        _log.Critical("SelfDestruct()");
        _tcpClient.Close();
    }

    private void InitCommands()
    {
        _commands.Add("client/connected", new ClientConnected());
    }

    public void addViewModel(ObservableObject viewModel)
    {
        ViewModel = viewModel;
    }

    public async Task AskForLoginAsync()
    {
        SendData(SendReplacedObject("username", Username, 1, SendReplacedObject(
            "password", Password, 1, "Requests\\connect.json"
        ))!);
    }

    public void ExitQueue()
    {
        try
        {
            var queueViewModel = (QueueViewModel)ViewModel;
            if (queueViewModel.JoinGame.CanExecute(null))
                queueViewModel.JoinGame.Execute(null);
        }
        catch (Exception e)
        {
            _log.Error(e, "Unable to exit queue");
            throw;
        }
    }
}