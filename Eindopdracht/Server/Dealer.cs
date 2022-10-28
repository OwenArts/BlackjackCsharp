using System.ComponentModel;
using Common;
using Server.CommandHandlers;

namespace Server;

public class Dealer
{
    private bool _timerStarted;
    private int _turnsPlayed;
    private readonly ServerSocket _parent;
    public Deck Deck { get; }
    private int _totalValue;
    private int _amountOfAces;
    private readonly List<ServerClient> _playingClients;

    public Dealer(ServerSocket parent)
    {
        _turnsPlayed = 0;
        _timerStarted = false;
        _parent = parent;
        Deck = new Deck();
        Deck.FillDeck();
        _playingClients = new List<ServerClient>();
    }

    public void StartTimer()
    {
        if(_timerStarted) return;
        _timerStarted = true;
        new Thread(TimeCounter).Start();
    }

    private void TimeCounter()
    {
        for (int i = 10; i >= 0; i--)
        {
            Thread.Sleep(1000);
        }

        _timerStarted = false;
        StartDealing();
    }

    private void StartDealing()
    {
        foreach (var client in _parent.Clients)
        {
            if(!client.IsPlaying && client.Bet > 0) return;
            _playingClients.Add(client);
        }
        
        for (var i = 0; i < 2; i++)
        {
            foreach (var client in _playingClients)
            {
                client.GiveCard(Deck.GetRandomCard());
                Thread.Sleep(1000);
            }
            
            GiveCardToSelf(Deck.GetRandomCard());
            Thread.Sleep(1000);
        }
        GiveTurn();
    }

    public void GiveTurn()
    {
        if (_turnsPlayed >= _playingClients.Count)
        {
            DealerPlay();
            return;
        }
        
        _playingClients[_turnsPlayed].NotifyTurn();
        _turnsPlayed++;
    }

    public void DisconnectClient(ServerClient client)
    {
        _playingClients.Remove(client);
    }

    private void GiveCardToSelf(Card card)
    {
        _totalValue += card.Value;
        if (card.Piece == 14) _amountOfAces++;

        while (_totalValue > 21 && _amountOfAces > 0)
        {
            _totalValue -= 10;
            _amountOfAces--;
        }
        
        _parent.GiveDealerCard(card.Piece, card.Suite, _totalValue);
    }

    private void DealerPlay()
    {
        while (_totalValue < 17)
        {
            GiveCardToSelf(Deck.GetRandomCard());
        }
        _parent.CalculateWinners();
    }
}