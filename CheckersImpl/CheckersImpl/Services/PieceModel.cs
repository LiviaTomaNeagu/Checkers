﻿using Newtonsoft.Json;
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
        private bool _isSelected;
        private TileModel _currentTile;
        private Player _player;
        public bool alreadyJumped = false;

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
            set
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

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        [JsonIgnore]

        public TileModel CurrentTile
        {
            get => _currentTile;
            set
            {
                if (_currentTile != value)
                {
                    _currentTile = value;
                    OnPropertyChanged(nameof(CurrentTile));
                }
            }
        }

        public Player Player
        {
            get => _player;
            set
            {
                if (_player != value)
                {
                    _player = value;
                    OnPropertyChanged(nameof(Player));
                }
            }
        }

        // Constructor
        public PieceModel(int row, int column, SolidColorBrush color, TileModel currentTile, Player player)
        {
            Row = row;
            Column = column;
            Color = color;
            IsKing = false;
            IsSelected = false;
            CurrentTile = currentTile;
            Player = player;
        }

        public PieceModel(SolidColorBrush color, bool isKing, int row, int column, TileModel currentTile)
        {
            Color = color;
            IsKing = isKing;
            Row = row;
            Column = column;
            IsSelected = false;
            CurrentTile = currentTile;
        }

        public PieceModel()
        {
        }

        // Method to crown the piece a king
        public void CrownPiece()
        {
            Color light = System.Windows.Media.Color.FromArgb(0xFF, 0xD1, 0xCB, 0xBD);
            Color dark = System.Windows.Media.Color.FromArgb(0xFF, 0xA4, 0x83, 0x74);
            SolidColorBrush lightbrown = new SolidColorBrush(light);
            SolidColorBrush darkbrown = new SolidColorBrush(dark);
            IsKing = true;
            if (Player == Player.PlayerOne)
            {
                Color = lightbrown;
            }
            else
            {
                Color = darkbrown;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
