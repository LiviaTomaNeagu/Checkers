using System.Windows.Media;

public class TileModel
{
    public int Row { get; set; }
    public int Column { get; set; }
    public Color Color { get; set; }
    public bool IsOccupied { get; set; }

    public TileModel(int row, int column, string color)
    {
        Row = row;
        Column = column;
        Color = GetColorFromString(color);
        IsOccupied = false;
    }

    public Color GetColorFromString(string color)
    {
        switch (color.ToLower())
        {
            case "lightbrown":
                return (Color)ColorConverter.ConvertFromString("#B79F6F");
            case "darkbrown":
                return (Color)ColorConverter.ConvertFromString("#826D42");
            default:
                return Colors.Transparent;
        }
    }

    public Color GetColor()
    {
        return Color;
    }
}
