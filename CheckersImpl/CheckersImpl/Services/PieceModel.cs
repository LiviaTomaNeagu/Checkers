using System;
using System.ComponentModel;
using System.Windows.Media;

namespace CheckersImpl.Services
{
    public class PieceModel : INotifyPropertyChanged
    {
        private SolidColorBrush _color;
        private bool _isKing;
        private int _row;
        private int _column;
        private bool _isVisible;

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

        public bool IsKing
        {
            get => _isKing;
            private set // Made private as it's set internally
            {
                if (_isKing != value)
                {
                    _isKing = value;
                    OnPropertyChanged(nameof(IsKing));
                }
            }
        }

        public int Row
        {
            get => _row;
            set
            {
                if (_row != value)
                {
                    _row = value;
                    OnPropertyChanged(nameof(Row));
                }
            }
        }

        public int Column
        {
            get => _column;
            set
            {
                if (_column != value)
                {
                    _column = value;
                    OnPropertyChanged(nameof(Column));
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged(nameof(IsVisible));
                }
            }
        }
        // Indicates if the piece has been crowned a king
        public bool IsSelected { get; set; }
        public TileModel CurrentTile { get; set; }

        // Constructor
        public PieceModel(int row, int column, SolidColorBrush color, TileModel currentTile)
        {
            Row = row;
            Column = column;
            Color = color;
            IsKing = false;
            IsVisible = true;
            IsSelected = false;
            CurrentTile = currentTile;
        }

        public PieceModel(SolidColorBrush color, bool isKing, int row, int column, bool isVisible)
        {
            Color = color;
            IsKing = isKing;
            Row = row;
            Column = column;
            IsVisible = isVisible;
        }

        // Method to crown the piece a king
        public void CrownPiece()
        {
            IsKing = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
