using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersImpl.Services
{
    internal class GameService
    {
        public bool IsValidMove(PieceModel selectedPiece, TileModel destinationTile)
        {
            return destinationTile.Row == selectedPiece.Row - 1
                && (destinationTile.Column == selectedPiece.Column - 1 
                || destinationTile.Column == selectedPiece.Column + 1)
                && !destinationTile.IsOccupied;
        }

        public void MovePiece(PieceModel selectedPiece, TileModel sourceTile, TileModel destinationTile)
        {
            if(!IsValidMove(selectedPiece, destinationTile))
            {
                throw new InvalidOperationException("Invalid move");
            }
            else
            {
                sourceTile.Piece = null;
                sourceTile.IsOccupied = false;
                destinationTile.Piece = selectedPiece;
                destinationTile.IsOccupied = true;
                selectedPiece.Row = destinationTile.Row;
                selectedPiece.Column = destinationTile.Column;
            }
        }
    }
}
