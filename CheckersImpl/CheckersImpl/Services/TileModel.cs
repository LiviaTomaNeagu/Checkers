using CheckersImpl.Services;
using System.ComponentModel;
using System.Windows.Media;

public class TileModel : INotifyPropertyChanged
{
    public int Row { get; set; }
    public int Column { get; set; }

    private SolidColorBrush _color;
    public SolidColorBrush Color
    {
        get => _color;
        set
        {
            if (_color != value)
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
    }
    public bool IsOccupied { get; set; }
    private PieceModel _piece;
    public PieceModel Piece
    {
        get => _piece;
        set
        {
            if (_piece != value)
            {
                _piece = value;
                OnPropertyChanged(nameof(Piece));
            }
        }
    }

    // Implementation of INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public bool IsSelected { get; set; }

    public TileModel(int row, int column, SolidColorBrush color)
    {
        Row = row;
        Column = column;
        Color = color;
        IsOccupied = false;
        Piece = null;
        IsSelected = false;
    }

    public  TileModel()
    {}
}
