namespace Common;

public class Card
{
    public int Piece { get; }
    public int Value { get; }
    public int Suite { get; }

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
}