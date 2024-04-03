using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CheckersImpl.Services
{
    public class BoardModel
    {
        public TileModel[,] myBoard { get; private set; }

        public BoardModel()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            myBoard = new TileModel[8, 8];
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    string color = (row + col) % 2 == 0 ? "lightbrown" : "darkbrown";
                    myBoard[row, col] = new TileModel(row, col, GetColorFromString(color));
                }
            }
        }

        private SolidColorBrush GetColorFromString(string color)
        {
            switch (color)
            {
                case "lightbrown":
                    return new SolidColorBrush(Colors.Pink);
                case "darkbrown":
                    return new SolidColorBrush(Colors.Plum);
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
