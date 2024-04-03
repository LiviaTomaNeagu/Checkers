using System;

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
        private BoardModel _boardModel;
        private FileService _fileService;
        private StatisticsService _statisticsService;
        public Player CurrentTurn { get; private set; }

        public GameService(FileService fileService, StatisticsService statisticsService)
        {
            _fileService = fileService;
            _statisticsService = statisticsService;

            _boardModel = new BoardModel();
        }

        public GameService()
        {
            _fileService = new FileService();
            _statisticsService = new StatisticsService();
            _boardModel = new BoardModel();
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
            _fileService.SaveGame(_boardModel, CurrentTurn);
        }

        public void LoadGame()
        {
            //// Use FileService to load the game state from a file
            GameLoadResult loadResult = _fileService.LoadGame();
            //CurrentTurn = loadResult.CurrentPlayer;
            _boardModel.myPieces = loadResult.PieceModel;
            //// Notify that the game state has changed
            OnGameStateChanged();
        }

        public bool ValidateMove(PieceModel piece, TileModel targetTile)
        {
            // Validate the move based on the rules of Checkers
            // This should involve checking for valid jump moves, king moves, etc.
            // Return true if the move is valid; otherwise, false
            return true; // Placeholder
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
    }
}
