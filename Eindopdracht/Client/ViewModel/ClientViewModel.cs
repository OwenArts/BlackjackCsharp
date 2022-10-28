using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Common;
using MvvmHelpers;

namespace Client.ViewModel;

public class ClientViewModel : ObservableObject
{


    private Client_ _client;
    private Log _log = new Log(typeof(ClientViewModel));

    private ObservableCollection<string> _dealerCards;
    private ObservableCollection<string> _player1Cards;
    private ObservableCollection<string> _player2Cards;
    private ObservableCollection<string> _player3Cards;
    private ObservableCollection<string> _player4Cards;
    // public ICommand EmergencyStop { get; }
    
    // public ObservableCollection<string> _chatMessages;
    
    private string _username;
    private string _score;

    public ClientViewModel(Client_ client)
    {
        _client = client;
        _client.addViewModel(this);
        _score = "0";
        // _chatMessages = new ObservableCollection<string>();
        // EmergencyStop = new EmergencyStopCommand(_client, this);
    }
    
    public string CurrentUserName
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }
    
    public string Score
    {
        get => _score;
        set
        {
            _score = value;
            OnPropertyChanged();
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