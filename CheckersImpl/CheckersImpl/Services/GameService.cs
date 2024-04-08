﻿using System;
using System.Collections.ObjectModel;

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
        public Player CurrentTurn { get; private set; }

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
            // Validate the move based on the rules of Checkers
            // This should involve checking for valid jump moves, king moves, etc.
            // Return true if the move is valid; otherwise, false
            //return true; // Placeholder
        }

        public void MakeMove(PieceModel piece, TileModel targetTile)
        {
            //// Execute the move if valid and update the board state
            //if (ValidateMove(piece, targetTile))
            //{
            //    // Example: Move the piece to the targetTile
            //    // Update the piece's Tile property and the Tile's Piece property
            //    // Check for any captures or if the piece should be kinged
            //    // Update statistics if necessary
            //    _statisticsService.Update(piece, targetTile);

            //    // Notify that the game state has changed
            //    OnGameStateChanged();
            //}
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
                && !destinationTile.IsOccupied;
        }

        public bool IsValidMoveUpBottom(PieceModel selectedPiece, TileModel destinationTile)
        {
            return destinationTile.Row == selectedPiece.Row + 1
                && (destinationTile.Column == selectedPiece.Column + 1
                || destinationTile.Column == selectedPiece.Column - 1)
                && !destinationTile.IsOccupied;
        }

        public void MovePiece(PieceModel selectedPiece, TileModel destinationTile)
        {
            // Check if the move is valid based on the player's turn and the piece's movement direction
            //if ((CurrentTurn == Player.PlayerOne && IsValidMoveUpBottom(selectedPiece, destinationTile))
            //    || (CurrentTurn == Player.PlayerTwo && IsValidMoveBottomUp(selectedPiece, destinationTile)))
            if(ValidateMove(selectedPiece, destinationTile))
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
                if(piece.Row == 0)
                {
                    piece.CrownPiece();
                }
            }
            else if(CurrentTurn == Player.PlayerTwo)
            {
                if(piece.Row == 7)
                {
                    piece.CrownPiece();
                }
            }
        }
    }
}
