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
    public int TotalValue { get; set; }
    private int _amountOfAces;
    public List<ServerClient> PlayingClients { get; }

    public Dealer(ServerSocket parent)
    {
        _turnsPlayed = 0;
        _timerStarted = false;
        _parent = parent;
        Deck = new Deck();
        Deck.FillDeck();
        PlayingClients = new List<ServerClient>();
    }

    public void StartTimer()
    {
        if(_timerStarted) return;
        _timerStarted = true;
        new Thread(TimeCounter).Start();
    }

    private void TimeCounter()
    {
        for (int i = 15; i >= 0; i--)
        {
            _parent.SendCounterUpdate(i);
            Thread.Sleep(1000);
        }

        _timerStarted = false;
        PlayingClients.Clear();
        TotalValue = 0;
        _turnsPlayed = 0;
        _amountOfAces = 0;
        Deck.FillDeck();
        foreach (var client in _parent.Clients.Where(client => client.IsPlaying && client.Bet > 0))
        {
            PlayingClients.Add(client);
        }
        _parent.SendStartedUpdate();
        StartDealing();
    }

    private void StartDealing()
    {
        for (var i = 0; i < 2; i++)
        {
            foreach (var client in PlayingClients)
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
        if (_turnsPlayed >= PlayingClients.Count)
        {
            DealerPlay();
            return;
        }
        
        PlayingClients[_turnsPlayed].NotifyTurn();
        _turnsPlayed++;
    }

    public void DisconnectClient(ServerClient client)
    {
        PlayingClients.Remove(client);
    }

    private void GiveCardToSelf(Card card)
    {
        TotalValue += card.Value;
        if (card.Piece == 14) _amountOfAces++;

        while (TotalValue > 21 && _amountOfAces > 0)
        {
            TotalValue -= 10;
            _amountOfAces--;
        }
        
        _parent.GiveDealerCard(card.Piece, card.Suite, TotalValue);
    }

    private void DealerPlay()
    {
        while (TotalValue < 17)
        {
            GiveCardToSelf(Deck.GetRandomCard());
            Thread.Sleep(1000);
        }
        _parent.CalculateWinners();
    }
}