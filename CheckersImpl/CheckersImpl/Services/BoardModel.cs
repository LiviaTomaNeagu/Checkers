using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersImpl.Services
{
    public class BoardModel
    {
        public TileModel[,] Board { get; private set; }

        public BoardModel()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            Board = new TileModel[8, 8];

            // Initialize each square with a default tile
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    string color = (row + col) % 2 == 0 ? "lightbrown" : "darkbrown";
                    Board[row, col] = new TileModel(row, col, color);
                }
            }
        }

        // Other methods and properties related to the game board
    }
}
