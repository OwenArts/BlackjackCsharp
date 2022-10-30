using System.Collections.ObjectModel;
using MvvmHelpers;

namespace Client;

public class Player : ObservableObject
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private int _score;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<string> _cards;
    public ObservableCollection<string> Cards
    {
        get => _cards;
        set
        {
            _cards = value;
            OnPropertyChanged();
        }
    }

    public void AddCard(string card)
    {
        var cards = new ObservableCollection<string>(Cards) { card };
        Cards = cards;
    }

    public Player(string name = "")
    {
        _name = name;
        _score = 0;
        _cards = new ObservableCollection<string>();
    }
}