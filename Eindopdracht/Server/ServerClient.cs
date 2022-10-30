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

    private readonly Log _log = new(typeof(ServerClient));
    public string Username { get; set; }
    public bool IsPlaying { get; set; }
    public int Bet { get; set; }
    public int Money { get; set; }

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
        Bet = 0;
        Money = 1000;
    }

    public void Start()
    {
        _stream.BeginRead(_buffer, 0, 1024, OnRead, null);
    }

    public void SendMessage(JObject packet)
    {
        _log.Debug($"SendMessage: {packet}");
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
                _log.Debug($"OnRead: {data}");
                _totalBuffer = Array.Empty<byte>();

                if (_commands.ContainsKey(data["id"]!.ToObject<string>()!))
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

    public void SelfDestruct(bool disconnectSelf)
    {
        Parent.Clients.Remove(this);
        Parent.Dealer.DisconnectClient(this);
        IsPlaying = false;
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

        if (_totalValue <= 21) return;
        SendMessage(GetJson("Response\\gobust.json"));
        Parent.Dealer.GiveTurn();
    }

    public int PlaceBet(int bet)
    {
        if (bet > Money)
        {
            SendMessage(GetJson("Response\\invalidbet.json"));
            return -1;
        }

        Parent.Dealer.StartTimer();
        Bet = bet;
        return Bet;
    }

    public int DoubleDown(int? testBet = null)
    {
        if (testBet != null)
            Bet = testBet.Value;
        
        if (Bet * 2 > Money)
        {
            SendMessage(GetJson("Response\\invalidbet.json"));
            return Bet;
        }

        Bet *= 2;
        GiveCard(Parent.Dealer.Deck.GetRandomCard());
        Parent.Dealer.GiveTurn();
        return Bet;
    }

    public void Play()
    {
        List<string> activeClients = new();
        foreach (var client in Parent.Clients.Where(client => client.IsPlaying))
        {
            activeClients.Add(client.Username);
            client.SendMessage(SendReplacedObject("user", Username, 1, "Response\\clientconnect.json")!);
        }

        SendMessage(SendReplacedObject("clients", activeClients.ToArray(), 1, "Response\\returnclients.json")!);

        _log.Information("client " + Username + " can play");
        IsPlaying = true;

        int status;
        if (Parent.GameActive)
            status = 1;
        else 
            status = 0;
        

        SendMessage(SendReplacedObject("status", 0, 1, SendReplacedObject(
            "money", Money, 1, SendReplacedObject(
                "active", status, 1, "Response\\clientconnected.json"
            )
        ))!);
    }

    public void NotifyNextClient()
    {
        foreach (var client in Parent.Clients.Where(client => !client.IsPlaying))
        {
            try
            {
                client.Play();
                break;
            }
            catch (Exception)
            {
                _log.Error("could not write to this client");
            }
        }
    }

    public int CalculateWin(int amountDealer, int? testTotal = null)
    {
        if (testTotal != null)
            _totalValue = testTotal.Value;

        int winstatus;
        if ((amountDealer > _totalValue && amountDealer <= 21) || _totalValue > 21)
        {
            Money -= Bet;
            winstatus = 0;
        }
        else if (amountDealer < _totalValue || amountDealer > 21)
        {
            Money += Bet;
            winstatus = 1;
        }
        else
        {
            winstatus = 2;
        }

        SendMessage(SendReplacedObject("win", winstatus, 1, SendReplacedObject(
            "balance", Money, 1, "Response\\winstatus.json"
        ))!);
        Bet = 0;
        _totalValue = 0;
        _amountOfAces = 0;
        return winstatus;
    }

    private void InitCommands()
    {
        _commands.Add("server/connect", new ClientConnect());
        _commands.Add("server/disconnect", new Disconnect());
        _commands.Add("server/getclients", new Disconnect());
        _commands.Add("server/calldeck", new CallDeck());
        _commands.Add("server/doubledown", new DoubleDown());
        _commands.Add("server/requestcard", new RequestCard());
        _commands.Add("server/placebet", new PlacedBet());
        _commands.Add("client/createaccount", new CreateAccount());
    }
}