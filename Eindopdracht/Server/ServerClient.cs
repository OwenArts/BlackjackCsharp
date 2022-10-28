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

    private int _totalValue;
    private int _amountOfAces;
    private byte[] _totalBuffer = Array.Empty<byte>();
    private readonly byte[] _buffer = new byte[1024];

    private readonly Log _log = new Log(typeof(ServerClient));
    public string Username { get; set; }
    public bool IsPlaying { get; set; }
    public int _bet;

    public ServerClient(TcpClient tcp, ServerSocket parent)
    {
        _totalValue = 0;
        _amountOfAces = 0;
        IsPlaying = false;
        Parent = parent;
        _tcp = tcp;
        _stream = _tcp.GetStream();
        _commands = new Dictionary<string, ICommandAction>();
        InitCommands();
        Username = "";
        _bet = 0;
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
        catch (Exception)
        {
            SelfDestruct(false);    
        }
    }

    public void SelfDestruct(bool disconnectSelf)
    {
        Parent.Clients.Remove(this);
        Parent.Dealer.DisconnectClient(this);
        if (!disconnectSelf) return;
        _stream.Close(1000);
        _tcp.Close();
    }

    public void NotifyTurn()
    {
        SendMessage(GetJson("Response\\giveturn.json"));
    }

    public void GiveCard(Card card)
    {
        _totalValue += card.Value;
        if (card.Piece == 14) _amountOfAces++;

        while (_totalValue > 21 && _amountOfAces > 0)
        {
            _totalValue -= 10;
            _amountOfAces--;
        }

        foreach (var client in Parent.Clients)
        {
            client.SendMessage(SendReplacedObject("user", Username, 1, SendReplacedObject(
                "piece", card.Piece, 1, SendReplacedObject(
                    "suite", card.Suite, 1, SendReplacedObject(
                        "value", _totalValue, 1, "Response\\givecard.json"
                    )
                )
            ))!);
        }
    }

    public void PlaceBet(int bet)
    {
        Parent.Dealer.StartTimer();
        _bet = bet;
    }

    public void DoubleDown()
    {
        _bet *= 2;
    }

    public void Play()
    {
        _log.Information("client " + Username + " can play");
        IsPlaying = true;
        SendMessage(SendReplacedObject("status", 0, 1, SendReplacedObject(
            "pos", Parent.Clients.Count, 1, "Response\\clientconnected.json"
        ))!);
    }

    public void NotifyNextClient()
    {
        foreach (var client in Parent.Clients.Where(client => !client.IsPlaying))
        {
            client.Play();
            break;
        }
    }

    private void InitCommands()
    {
        _commands.Add("server/connect", new ClientConnect());
        _commands.Add("server/disconnect", new Disconnect());
        _commands.Add("server/getclients", new Disconnect());
    }
}