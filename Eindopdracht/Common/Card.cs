namespace Common;

public class Card
{
    public int Piece { get; set; }
    public int Value { get; set; }
    public int Suite { get; set; }

    public Card(int piece, int suite)
    {
        Value = piece switch
        {
            > 10 and < 14 => 10,
            14 => 11,
            _ => piece
        };

        Piece = piece;
        Suite = suite;
    }

    public string NamedValue
    {
        get
        {
            var name = Piece switch
            {
                (14) => "Ace",
                (13) => "King",
                (12) => "Queen",
                (11) => "Jack",
                _ => Piece.ToString()
            };
            return name;
        }
    }

    public string NamedSuite
    {
        get
        {
            var name = Suite switch
            {
                0 => "Hearts",
                1 => "Spades",
                2 => "Diamonds",
                3 => "Clubs"
            };
            return name;
        }
    }
}