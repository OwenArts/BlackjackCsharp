using System.Net.Sockets;
using Common;
using Newtonsoft.Json.Linq;
using Server.CommandHandlers;
using static Common.Util;
using static Common.Cryptographer;

namespace Server;

public class ServerClient
{

    public ServerSocket Parent { get; }
    private readonly TcpClient _tcp;
    private readonly NetworkStream _stream;
    private readonly Dictionary<string, ICommandAction> _commands;

    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    public Log _log = new Log(typeof(ServerClient));
    public string Username { get; set; }

    public ServerClient(TcpClient tcp, ServerSocket parent)
    {
        Parent = parent;
        _tcp = tcp;
        _stream = _tcp.GetStream();
        _commands = new Dictionary<string, ICommandAction>();
        InitCommands();
        Username = "";
    }

    public void Start()
    {
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    public void SendMessage(JObject packet)
    {
        byte[] encryptedMessage = GetEncryptedMessage(packet);
        _stream.Write(encryptedMessage, 0, encryptedMessage.Length);
    }

    private void OnRead(IAsyncResult ar)
    {
        try
        {
            int rc = _stream.EndRead(ar);
            _totalBuffer = Concat(_totalBuffer, _buffer, rc);

            while (_totalBuffer.Length >= 4)
            {
                JObject data = GetDecryptedMessage(_totalBuffer);
                _log.Debug(data.ToString());
                _totalBuffer = Array.Empty<byte>();
                
                if(_commands.ContainsKey(data["id"]!.ToObject<string>()!))
                    _commands[data["id"]!.ToObject<string>()!].OnCommandReceived(data, this);
                break;
            }

            _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
        }
        catch (Exception e)
        {
            _log.Error(e, "OnRead() err");
            SelfDestruct(false);    
        }
    }

    public async void SelfDestruct(bool disconnectSelf)
    {
        Parent.Clients.Remove(this);
        if (!disconnectSelf) return;
        _stream.Close(1000);
        _tcp.Close();
    }

    private void InitCommands()
    {
        _commands.Add("server/connect", new ClientConnect());
    }
}