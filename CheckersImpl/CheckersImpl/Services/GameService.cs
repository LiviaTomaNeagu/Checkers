using CheckersImpl.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public bool AllowMultipleJumps = false;
        public bool EndTurn = false;
        public int PlayerOnePieces;
        public int PlayerTwoPieces;

        public GameService(ObservableCollection<PieceModel> Pieces)
        {
            _fileService = new FileService();
            _statisticsService = new StatisticsService();
            this.Pieces = Pieces;
            CurrentTurn = Player.PlayerOne;
            PlayerTwoPieces = 12;
            PlayerOnePieces = 12;
        }

        public MessageBoxResult DeclareDraw()
        {
            _statisticsService.UpdateStatistics(Player.None);
            return MessageBox.Show("The game is a draw!\n Do you want to start a new game?", "DRAW", MessageBoxButton.OKCancel);  
        }

        public void SaveGame()
        {
            // Use FileService to save the current game state to a file
            _fileService.SaveGame(Pieces, CurrentTurn, AllowMultipleJumps);
        }

        public void LoadGame()
        {
            // Use FileService to load the game state from a file
            GameLoadResult loadResult = _fileService.LoadGame();
            if(loadResult != null)
            {
                CurrentTurn = loadResult.CurrentPlayer;
                AllowMultipleJumps = loadResult.AllowMultipleJumps;
                foreach(var piece in Pieces)
                {
                    piece.CurrentTile.Piece = null;
                    piece.CurrentTile.IsOccupied = false;
                }
                Pieces.Clear(); // Clear the existing collection
                foreach (var piece in loadResult.PieceModel)
                {
                    Pieces.Add(piece); // Add elements from loadResult.PieceModel to the existing collection
                    
                }
            }
            // Notify that the game state has changed
            OnGameStateChanged();
        }

        public string ShowStatistics()
        {
            return _statisticsService.ShowStatistics();
        }

        public bool ValidateMove(PieceModel piece, TileModel targetTile)
        {
            if (!piece.IsKing)
            {
                if (CurrentTurn == Player.PlayerOne && piece.Player == Player.PlayerOne)
                {
                    return IsValidMoveUpBottom(piece, targetTile);
                }
                else if (CurrentTurn == Player.PlayerTwo && piece.Player == Player.PlayerTwo)
                {
                    return IsValidMoveBottomUp(piece, targetTile);
                }
                return false;
            }
            else
            {
                if (CurrentTurn == piece.Player)
                {
                    return IsValidMoveUpBottom(piece, targetTile) || IsValidMoveBottomUp(piece, targetTile);
                }
                return false;
            }
        }

        public bool ValidateJump(PieceModel piece, TileModel targetTile)
        {
            // Calculate the row and column of the tile being jumped over
            int jumpedRow = (targetTile.Row > piece.Row) ? piece.Row + 1 : piece.Row - 1;
            int jumpedColumn = (targetTile.Column > piece.Column) ? piece.Column + 1 : piece.Column - 1;

            // Check if there's an opponent's piece in the tile being jumped over
            PieceModel jumpedPiece = Pieces.FirstOrDefault(p => p.Row == jumpedRow && p.Column == jumpedColumn);

            if (jumpedPiece != null && jumpedPiece.Player != CurrentTurn )
            {
                if (!piece.IsKing)
                {
                    if (CurrentTurn == Player.PlayerOne && piece.Player == Player.PlayerOne)
                    {
                        return IsValidJumpUpBottom(piece, targetTile);
                    }
                    else if (CurrentTurn == Player.PlayerTwo && piece.Player == Player.PlayerTwo)
                    {
                        return IsValidJumpBottomUp(piece, targetTile);
                    }
                    return false;
                }
                else
                {
                    if (CurrentTurn == piece.Player)
                    {
                        return IsValidJumpUpBottom(piece, targetTile) || IsValidJumpBottomUp(piece, targetTile);
                    }
                    return false;
                }
            }
            else
            {
                return false;
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

        public MessageBoxResult MovePiece(PieceModel selectedPiece, TileModel destinationTile)
        {
            if (selectedPiece.Player != CurrentTurn)
            {
                MessageBox.Show("Those are not your pieces.. Pick something else!");
                return MessageBoxResult.None;
            }
            if (ValidateMove(selectedPiece, destinationTile) && selectedPiece.alreadyJumped == false)
            {
                HandleMove(selectedPiece, destinationTile);
                SwitchTurns();
            }
            // Check if the move is a valid jump
            else if (ValidateJump(selectedPiece, destinationTile))
                {
                    // Calculate the row and column of the tile being jumped over
                    int jumpedRow = (destinationTile.Row > selectedPiece.Row) ? selectedPiece.Row + 1 : selectedPiece.Row - 1;
                    int jumpedColumn = (destinationTile.Column > selectedPiece.Column) ? selectedPiece.Column + 1 : selectedPiece.Column - 1;

                    // Check if there's an opponent's piece in the tile being jumped over
                    PieceModel jumpedPiece = Pieces.FirstOrDefault(p => p.Row == jumpedRow && p.Column == jumpedColumn);
                    if (jumpedPiece != null && jumpedPiece.Player != CurrentTurn)
                    {
                        // Remove the jumped piece from the collection
                        jumpedPiece.CurrentTile.Piece = null;
                        jumpedPiece.CurrentTile.IsOccupied = false;
                        if (jumpedPiece.Player == Player.PlayerOne)
                        {
                            PlayerOnePieces--;
                            if(WON() != Player.None)
                            {
                                return MessageBox.Show("Player Two wins!\n Do you want to start a new game?", "CONGRATULATIONS", MessageBoxButton.OKCancel);
                            }
                        }
                        else if (jumpedPiece.Player == Player.PlayerTwo)
                        {
                            PlayerTwoPieces--;
                            if(WON() != Player.None)
                            {
                                // Player One wins
                                return MessageBox.Show("Player One wins!\n Do you want to start a new game?","CONGRATULATIONS", MessageBoxButton.OKCancel);
                            }
                        }
                        selectedPiece.alreadyJumped = true;
                        HandleMove(selectedPiece, destinationTile);
                    }
                    if (AllowMultipleJumps == false)
                    {
                        selectedPiece.alreadyJumped = false;
                        SwitchTurns();
                    }
                }
                else
                {
                    MessageBox.Show("Can't go that way. Sorry.. Try again!");
                    return MessageBoxResult.None;
                }
            return MessageBoxResult.None;
        }

        private void HandleMove(PieceModel selectedPiece, TileModel destinationTile)
        {
            selectedPiece.CurrentTile.Piece = null;
            selectedPiece.CurrentTile.IsOccupied = false;
            destinationTile.Piece = selectedPiece;
            destinationTile.IsOccupied = true;
            selectedPiece.Row = destinationTile.Row;
            selectedPiece.Column = destinationTile.Column;
            selectedPiece.CurrentTile = destinationTile;
            CrownPiece(selectedPiece);
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

        public Player WON()
        {             
            if (PlayerOnePieces == 0)
            {
                _statisticsService.UpdateStatistics(Player.PlayerTwo);
                return Player.PlayerTwo;
            }
            else if (PlayerTwoPieces == 0)
            {
                _statisticsService.UpdateStatistics(Player.PlayerOne);
                return Player.PlayerOne;
            }
            return Player.None;
        }
    }
}
