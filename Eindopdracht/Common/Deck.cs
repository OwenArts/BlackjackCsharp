namespace Common;

public class Deck
{
    private readonly List<Card> _cards;

    public Deck()
    {
        _cards = new List<Card>();
    }

    public void FillDeck()
    {
        _cards.Clear();
        for (int i = 0; i < 52; i++)
        {
            var suite = (int)Math.Floor((decimal)i / 13);
            var piece = i % 13 + 2;
            _cards.Add(new Card(piece, suite));
        }
    }

    public Card GetRandomCard()
    {
        Card card = _cards[new Random().Next(_cards.Count - 1)];
        _cards.Remove(card);
        return card;
    }
}