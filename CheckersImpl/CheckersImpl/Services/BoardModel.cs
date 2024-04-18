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
        public PieceModel[] myPieces { get;  set; }

        public BoardModel()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            myBoard = new TileModel[8, 8];
            myPieces = new PieceModel[24]; // Assuming 12 pieces for each player
            int pieceIndex = 0;

            Color light = Color.FromArgb(0xEB, 0xF1, 0xED, 0xE6);
            Color dark = Color.FromArgb(0xFF, 0x3A, 0x2D, 0x28);
            SolidColorBrush lightpieces = new SolidColorBrush(light);
            SolidColorBrush darkpieces = new SolidColorBrush(dark);

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    // Determine the color of the tile based on its position
                    string color = (row + col) % 2 == 0 ? "lightbrown" : "darkbrown";
                    myBoard[row, col] = new TileModel(row, col, GetColorFromString(color));

                    // Place pieces on the appropriate tiles for the initial setup
                    // For a standard 8x8 checkers board, the first 3 rows and the last 3 rows are filled with pieces
                    // Pieces are placed on the dark squares
                    if (color == "darkbrown" && row < 3)
                    {
                        // Create a new piece and place it on the current tile
                        //PieceModel piece = new PieceModel(row, col, new SolidColorBrush(Colors.Purple)); // Use whatever color represents one set of pieces
                        PieceModel piece = new PieceModel(row, col, lightpieces, myBoard[row, col], Player.PlayerOne); // Use whatever color represents one set of pieces
                        myBoard[row, col].Piece = piece;
                        myPieces[pieceIndex++] = piece;
                        myBoard[row, col].IsOccupied = true;
                    }
                    else if (color == "darkbrown" && row >= 5)
                    {
                        // Create a new piece and place it on the current tile
                        PieceModel piece = new PieceModel(row, col, darkpieces, myBoard[row, col], Player.PlayerTwo); // Use whatever color represents the other set of pieces
                        myBoard[row, col].Piece = piece;
                        myPieces[pieceIndex++] = piece;
                        myBoard[row, col].IsOccupied = true;
                    }
                }
            }
        }

        private SolidColorBrush GetColorFromString(string color)
        {
            Color light = Color.FromArgb(0xEB, 0xEB, 0xE3, 0xDB);
            Color dark = Color.FromArgb(0xFF, 0xCB, 0xAD, 0x8D);
            SolidColorBrush lightbrown = new SolidColorBrush(light);
            SolidColorBrush darkbrown = new SolidColorBrush(dark);
            switch (color)
            {
                case "lightbrown":
                    return lightbrown;
                case "darkbrown":
                    return darkbrown;
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
