using CheckersImpl.Services;
using System.Windows.Media;

public class TileModel
{
    public int Row { get; set; }
    public int Column { get; set; }
    public SolidColorBrush Color { get; set; }
    public bool IsOccupied { get; set; }
    public PieceModel Piece { get; set; }

    public TileModel(int row, int column, SolidColorBrush color)
    {
        Row = row;
        Column = column;
        Color = color;
        IsOccupied = false;
        Piece = null;
    }
}
