using CheckersImpl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersImpl.ViewModels
{
    internal class BoardVM
    {
        public BoardModel BoardModel { get; }

        public ObservableCollection<TileModel> Board { get; set; }

        public BoardVM()
        {
            // Create an instance of BoardModel
            BoardModel = new BoardModel();

            // Initialize the view model's board based on the BoardModel
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Initialize the ObservableCollection
            Board = new ObservableCollection<TileModel>();

            // Loop through each cell in the BoardModel and create a TileModel for each one
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    // Get the color of the tile from the BoardModel (assuming it's represented as an int for simplicity)
                    string colorString = BoardModel.Board[row, column].Color.ToString();

                    // Convert the color integer to a string representation (e.g., "lightbrown", "darkbrown", etc.)
                    //string colorString = GetColorString(color);

                    // Create a new TileModel and add it to the ObservableCollection
                    Board.Add(new TileModel(row, column, colorString));
                }
            }
        }
    }
}
