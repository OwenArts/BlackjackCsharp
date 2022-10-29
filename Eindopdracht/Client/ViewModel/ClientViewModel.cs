using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Common;
using MvvmHelpers;

namespace Client.ViewModel;

public class ClientViewModel : ObservableObject
{
    private readonly Client_ _client;
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


        _client = client;
        _client.addViewModel(this);
        _self = new Player(_client.Username);
        _player1 = new Player(player1);
        _player2 = new Player(player2);
        _player3 = new Player(player3);
        _dealer = new Player("Dealer");
        _players = new List<Player>{ _self, _player1, _player2, _player3, _dealer };
    }

    public void UpdateCards(string name, string card, int value)
    {
        foreach (var player in _players)
        {
            if(player.Name != name) continue;
            player.AddCard(card);
            player.Score = value;
        }
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