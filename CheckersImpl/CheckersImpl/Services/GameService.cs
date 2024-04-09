using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CheckersImpl.Services
{
    public enum Player
    {
        None,
        PlayerOne,
        PlayerTwo
    }

    public class GameService
    {
        public ObservableCollection<PieceModel> Pieces;

        private FileService _fileService;
        private StatisticsService _statisticsService;
        public Player CurrentTurn { get; set; }


        public GameService(ObservableCollection<PieceModel> Pieces)
        {
            _fileService = new FileService();
            _statisticsService = new StatisticsService();
            this.Pieces = Pieces;
            CurrentTurn = Player.PlayerOne;
        }

 
        public void StartNewGame()
        {
            //// Initialize or reset the game board to its starting state
            //_boardModel = new BoardModel();
            //// Possible initialization of statistics or other relevant game start actions
            //_statisticsService.Reset();
            //// Notify that the game state has changed
            //OnGameStateChanged();
        }

        public void SaveGame()
        {
            //// Use FileService to save the current game state to a file
           // _fileService.SaveGame(_boardModel, CurrentTurn);
        }

        public void LoadGame()
        {
            //// Use FileService to load the game state from a file
            GameLoadResult loadResult = _fileService.LoadGame();
            if(loadResult != null)
            {//CurrentTurn = loadResult.CurrentPlayer;
                Pieces.Clear(); // Clear the existing collection
                foreach (var piece in loadResult.PieceModel)
                {
                    Pieces.Add(piece); // Add elements from loadResult.PieceModel to the existing collection
                }
            }
            //// Notify that the game state has changed
            OnGameStateChanged();
        }

        public bool ValidateMove(PieceModel piece, TileModel targetTile)
        {
            if (!piece.IsKing)
            {
                if (CurrentTurn == Player.PlayerOne)
                {
                    return IsValidMoveUpBottom(piece, targetTile);
                }
                else if (CurrentTurn == Player.PlayerTwo)
                {
                    return IsValidMoveBottomUp(piece, targetTile);
                }
                return false;
            }
            else
            {
                // King piece can move in any direction
                return true;
            }
        }

        public bool ValidateJump(PieceModel piece, TileModel targetTile)
        {
            if (!piece.IsKing)
            {
                if (CurrentTurn == Player.PlayerOne)
                {
                    return IsValidJumpUpBottom(piece, targetTile);
                }
                else if (CurrentTurn == Player.PlayerTwo)
                {
                    return IsValidJumpBottomUp(piece, targetTile);
                }
                return false;
            }
            else
            {
                // King piece can jump in any direction
                return true;
            }
        }

        private void OnGameStateChanged()
        {
            // Notify any subscribers that the game state has changed
            // This can be done using events or other messaging patterns
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }

        // Method to switch turns
        public void SwitchTurns()
        {
            if (CurrentTurn == Player.PlayerOne)
            {
                CurrentTurn = Player.PlayerTwo;
            }
            else if (CurrentTurn == Player.PlayerTwo)
            {
                CurrentTurn = Player.PlayerOne;
            }

            // Raise an event or notify the UI that the turn has switched
        }

        public event EventHandler GameStateChanged;
        public bool IsValidMoveBottomUp(PieceModel selectedPiece, TileModel destinationTile)
        {
            return destinationTile.Row == selectedPiece.Row - 1
                && (destinationTile.Column == selectedPiece.Column - 1
                || destinationTile.Column == selectedPiece.Column + 1)
                && destinationTile.IsOccupied == false;
        }

        public bool IsValidMoveUpBottom(PieceModel selectedPiece, TileModel destinationTile)
        {
            return destinationTile.Row == selectedPiece.Row + 1
                && (destinationTile.Column == selectedPiece.Column + 1
                || destinationTile.Column == selectedPiece.Column - 1)
                && destinationTile.IsOccupied == false; 
        }

        public bool IsValidJumpUpBottom(PieceModel selectedPiece, TileModel destinationTile)
        {
            return destinationTile.Row == selectedPiece.Row + 2
                && (destinationTile.Column == selectedPiece.Column + 2
                || destinationTile.Column == selectedPiece.Column - 2)
                && destinationTile.IsOccupied == false;
        }

        public bool IsValidJumpBottomUp(PieceModel selectedPiece, TileModel destinationTile)
        {
            return destinationTile.Row == selectedPiece.Row - 2
                && (destinationTile.Column == selectedPiece.Column + 2
                || destinationTile.Column == selectedPiece.Column - 2)
                && destinationTile.IsOccupied == false;
        }

        public void MovePiece(PieceModel selectedPiece, TileModel destinationTile)
        {
            // Check if the move is valid based on the player's turn and the piece's movement direction
            if(ValidateJump(selectedPiece, destinationTile))
            {
                // Calculate the row and column of the tile being jumped over
                int jumpedRow = (destinationTile.Row > selectedPiece.Row) ? selectedPiece.Row + 1 : selectedPiece.Row - 1;
                int jumpedColumn = (destinationTile.Column > selectedPiece.Column) ? selectedPiece.Column + 1 : selectedPiece.Column - 1;

                // Check if there's an opponent's piece in the tile being jumped over
                PieceModel jumpedPiece = Pieces.FirstOrDefault(p => p.Row == jumpedRow && p.Column == jumpedColumn); 
                if (CurrentTurn == Player.PlayerTwo && jumpedPiece.Player == Player.PlayerOne 
                    || CurrentTurn == Player.PlayerOne && jumpedPiece.Player == Player.PlayerTwo)
                {
                    jumpedPiece.CurrentTile.Piece = null;
                    jumpedPiece.CurrentTile.IsOccupied = false;
                }
            }
            if(ValidateMove(selectedPiece, destinationTile) || ValidateJump(selectedPiece, destinationTile))
            {
                // Update the source and destination tiles with the moved piece
                selectedPiece.CurrentTile.Piece = null;
                selectedPiece.CurrentTile.IsOccupied = false;
                destinationTile.Piece = selectedPiece;
                destinationTile.IsOccupied = true;
                // Update the piece's row, column, and current tile
                selectedPiece.Row = destinationTile.Row;
                selectedPiece.Column = destinationTile.Column;
                selectedPiece.CurrentTile = destinationTile;
                // Check if the piece should be crowned
                CrownPiece(selectedPiece);
                SwitchTurns();
            }
            else
            {
                throw new InvalidOperationException("Invalid move");
            }
        }

        public void CrownPiece(PieceModel piece)
        {
            if(CurrentTurn == Player.PlayerOne)
            {
                if(piece.Row == 7)
                {
                    piece.CrownPiece();
                }
            }
            else if(CurrentTurn == Player.PlayerTwo)
            {
                if(piece.Row == 0)
                {
                    piece.CrownPiece();
                }
            }
        }
    }
}
