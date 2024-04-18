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
            myPieces = new PieceModel[24];
            int pieceIndex = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    string color = (row + col) % 2 == 0 ? "lightbrown" : "darkbrown";
                    myBoard[row, col] = new TileModel(row, col, GetColorFromString(color));

                    if (color == "darkbrown" && row < 3)
                    {
                        PieceModel piece = new PieceModel(row, col, new SolidColorBrush(Colors.White), myBoard[row, col], Player.PlayerOne);
                        myBoard[row, col].Piece = piece;
                        myPieces[pieceIndex++] = piece;
                        myBoard[row, col].IsOccupied = true;
                    }
                    else if (color == "darkbrown" && row >= 5)
                    {
                        PieceModel piece = new PieceModel(row, col, new SolidColorBrush(Colors.Black), myBoard[row, col], Player.PlayerTwo);
                        myBoard[row, col].Piece = piece;
                        myPieces[pieceIndex++] = piece;
                        myBoard[row, col].IsOccupied = true;
                    }
                }
            }
        }

        private SolidColorBrush GetColorFromString(string color)
        {
            switch (color)
            {
                case "lightbrown":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffce9e"));
                case "darkbrown":
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d18b47"));
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
