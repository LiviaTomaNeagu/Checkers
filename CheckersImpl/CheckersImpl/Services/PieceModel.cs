using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CheckersImpl.Services
{
    public class PieceModel
    {
        // Color of the piece, for example, "white" or "black"
        public SolidColorBrush Color { get; private set; }

        // Indicates if the piece has been crowned a king
        public bool IsKing { get; private set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsVisible { get; set; }

        // Constructor
        public PieceModel(int row, int column, SolidColorBrush color)
        {
            Row = row;
            Column = column;
            Color = color;
            IsKing = false;
            IsVisible = true;
        }

        // Method to crown the piece a king
        public void CrownPiece()
        {
            IsKing = true;
        }

        // You might also include methods to move the piece,
        // which would be called by the game logic.
    }

}
