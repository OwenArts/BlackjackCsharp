using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Client.Command;
using Common;
using MvvmHelpers;

namespace Client.ViewModel;

public class ClientViewModel : ObservableObject
{
    public Client_ Client { get; }
    private readonly Log _log = new Log(typeof(ClientViewModel));

    private readonly Player _self;
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly Player _player3;
    private readonly Player _dealer;

    public Player Self => _self;
    public Player Player1 => _player1;
    public Player Player2 => _player2;
    public Player Player3 => _player3;
    public Player Dealer => _dealer;
    private List<Player> _players;


    private bool _gameStarted;
    private bool _hasTurn;
    private bool _firstTurn;
    private string _middleMessage = "plaats uw inleg";
    private int _money = 0;
    private string _bet = "";
    
    public bool GameStarted
    {
        get => !_gameStarted;
        set
        {
            _gameStarted = value;
            OnPropertyChanged();
        }
    }

    public bool HasTurn
    {
        get => _hasTurn;
        set
        {
            _hasTurn = value;
            OnPropertyChanged();
        }
    }

    public bool FirstTurn
    {
        get => _firstTurn;
        set
        {
            _firstTurn = value;
            OnPropertyChanged();
        }
    }

    public string MiddleMessage
    {
        get => _middleMessage;
        set
        {
            _middleMessage = value;
            OnPropertyChanged();
        }
    }

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnPropertyChanged();
        }
    }

    public string Bet
    {
        get => _bet;
        set
        {
            _bet = value;
            OnPropertyChanged();
        }
    }

    public ICommand Hit { get; }
    public ICommand Stand { get; }
    public ICommand DoubleDown { get; }
    public ICommand BetC { get; }

    public ClientViewModel(Client_ client)
    {
        var player1 = "";
        var player2 = "";
        var player3 = "";
        var otherPlayers = client.OtherPlayers;
        _log.Information("Client got to here");
        if (otherPlayers.Length > 0)
            player1 = otherPlayers[0];
        if (otherPlayers.Length > 1)
            player2 = otherPlayers[1];
        if (otherPlayers.Length > 2)
            player3 = otherPlayers[2];
        
        _log.Information("Client finished defining players");

        
        Client = client;
        Client.AddViewModel(this);
        _self = new Player(Client.Username);
        _player1 = new Player(player1);
        _player2 = new Player(player2);
        _player3 = new Player(player3);
        _dealer = new Player("Dealer");
        _players = new List<Player>{ _self, _player1, _player2, _player3, _dealer };

        if (Client.GameActive)
        {
            _gameStarted = true;
            MiddleMessage = "Wacht alstublieft totdat het huidige spel voorbij is";
        }
        else
        {
            _gameStarted = false;            
        }
        
        _hasTurn = false;
        _firstTurn = false;
        Money = Client.Balance;

        Hit = new HitCommand(this);
        Stand = new StandCommand(this);
        DoubleDown = new DoubleDownCommand(this);
        BetC = new BetCommand(this);
    }

    public void UpdateCards(string name, string card, int value)
    {
        foreach (var player in _players.Where(player => player.Name == name))
        {
            player.AddCard(card);
            player.Score = value;
        }
    }

    public void Reset()
    {
        foreach (var player in _players)
        {
            player.Cards.Clear();
            player.Score = 0;
        }
        GameStarted = false;
    }


    // public Patient CurrentUser
    // {
    //     get => _currentUser;
    //     set
    //     {
    //         
    //         _currentUser = value;
    //         if (CurrentUser != null)
    //         {
    //             CurrentUserName = CurrentUser.Username;
    //         }
    //         
    //         //OnPropertyChanged(nameof(CurrentUser));
    //         
    //         ChartDataSpeed = new SeriesCollection()
    //         {
    //             new LineSeries()
    //             {
    //                 Fill = Brushes.Transparent,
    //                 Stroke = Brushes.DarkSeaGreen,
    //                 PointGeometrySize = 0,
    //                 LineSmoothness = 1.00,
    //                 Values = _currentUser.speedData
    //             }
    //         };
    //         
    //         ChartDataBPM = new SeriesCollection()
    //         {
    //             new LineSeries()
    //             {
    //                 Fill = Brushes.Transparent,
    //                 Stroke = Brushes.LightCoral,
    //                 PointGeometrySize = 0,
    //                 LineSmoothness = 1.00,
    //                 Values = _currentUser.bpmData
    //             }
    //         };
    //         OnPropertyChanged();
    //     }
    // }
    //
    // public ObservableCollection<string> ChatMessages
    // {
    //     get => _chatMessages;
    //     set
    //     {
    //         _chatMessages = value;
    //         OnPropertyChanged();
    //     } 
    // }
    //
    // public ObservableCollection<Patient> Patients
    // {
    //     get => _patients;
    //     set => _patients = value;
    // }
    //
    // public string TextBoxChatMessage
    // {
    //     get => _chatMessage;
    //     set => _chatMessage = value;
    // }
    //
    // public int Resistance
    // {
    //     get => _resistance;
    //     set => _resistance = value;
    //        
    // }
    //
    // public int BPM
    // {
    //     get => _currentUser.currentBPM;
    //     set
    //     {
    //         _currentUser.currentBPM = value;
    //         OnPropertyChanged();
    //     }
    // }
    //
    // public float Speed
    // {
    //     get => _currentUser.currentSpeed;
    //     set
    //     {
    //         _currentUser.currentSpeed = value;
    //         OnPropertyChanged();
    //     }
    // }
    //
    // public float Distance
    // {
    //     get => _currentUser.currentDistance;
    //     set
    //     {
    //         _currentUser.currentDistance = value;
    //         OnPropertyChanged();
    //     }
    // }
    //
    // public TimeSpan ElapsedTime
    // {
    //     get => _currentUser.currentElapsedTime;
    //     set
    //     {
    //         _currentUser.currentElapsedTime = value;
    //         OnPropertyChanged();
    //     }
    // }
    //
    // public SeriesCollection ChartDataSpeed
    // {
    //     get => _chartDataSpeed;
    //     set
    //     {
    //         _chartDataSpeed = value;
    //         OnPropertyChanged();
    //     }
    // }
    //
    // public SeriesCollection ChartDataBPM
    // {
    //     get => _chartDataBPM;
    //     set
    //     {
    //         _chartDataBPM = value;
    //         OnPropertyChanged();
    //     }
    // }
}