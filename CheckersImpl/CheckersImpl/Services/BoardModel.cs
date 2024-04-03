using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersImpl.Services
{
    public class BoardModel
    {
        public int[,] BoardState { get; private set; }

        public BoardModel()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Initialize the board with the starting state
            BoardState = new int[8, 8];
            // Logic to set up the initial state of the board
        }

        // Other methods and properties related to the game board
    }
}

