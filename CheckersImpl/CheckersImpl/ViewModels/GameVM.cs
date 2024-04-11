using System.ComponentModel;
using System.Windows.Input;
using CheckersImpl.Services;
using CheckersImpl.Commands;
using System.Data;

namespace CheckersImpl.ViewModels
{
    public class GameVM : INotifyPropertyChanged
    {
        private GameService _gameService;

        private bool _allowMultipleJumps = false;
        public bool AllowMultipleJumps
        {
            get => _gameService.AllowMultipleJumps;
            set
            {
                if (_allowMultipleJumps != value)
                {
                    _allowMultipleJumps = value;
                    _gameService.AllowMultipleJumps = value;
                    OnPropertyChanged(nameof(AllowMultipleJumps));
                }
            }
        }

        private BoardVM _boardVM;

        public BoardVM boardVM
        {
            get { return _boardVM; }
            set
            {
                if (_boardVM != value)
                {
                    _boardVM = value;
                    OnPropertyChanged(nameof(BoardVM));
                    if (_boardVM != null && _boardVM.DestinationTile != null)
                    {
                        _boardVM.DestinationTile.PropertyChanged += DestinationTile_PropertyChanged;
                    }

                }
            }
        }

        // Example property bound to by the view
        public Player CurrentPlayer
        {
            get => _gameService.CurrentTurn;
            set
            {
                if (_gameService.CurrentTurn != value)
                {
                    CurrentPlayer = value;
                    _gameService.CurrentTurn = value;
                    OnPropertyChanged(nameof(CurrentPlayer));
                }
                
            }
        }

        public int PlayerOnePieces
        {
            get => _gameService.PlayerOnePieces;
            set
            {
                if (_gameService.PlayerOnePieces != value)
                {
                    _gameService.PlayerOnePieces = value;
                    PlayerOnePieces = value;
                    OnPropertyChanged(nameof(PlayerOnePieces));
                }
            }
        }

        public int PlayerTwoPieces
        {
            get => _gameService.PlayerTwoPieces;
            set
            {
                if (_gameService.PlayerTwoPieces != value)
                {
                    PlayerTwoPieces = value;
                    _gameService.PlayerTwoPieces = value;
                    OnPropertyChanged(nameof(PlayerTwoPieces));
                }
            }
        }

        // Commands
        public ICommand NewGameCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }

        private ICommand _loadGameCommand;
        public ICommand LoadGameCommand
        {
            get { return _loadGameCommand; }
            set
            {
                if (_loadGameCommand != value)
                {
                    _loadGameCommand = value;
                    OnPropertyChanged(nameof(LoadGameCommand));
                }
            }
        }

        private ICommand _statisticsCommand;
        public ICommand StatisticsCommand
        {
            get { return _statisticsCommand; }
            set
            {
                if (_statisticsCommand != value)
                {
                    _statisticsCommand = value;
                    OnPropertyChanged(nameof(StatisticsCommand));
                }
            }
        }

        private string statistics;
        public string Statistics
        {
            get => statistics;
            set
            {
                if (statistics != value)
                {
                    statistics = value;
                    OnPropertyChanged(nameof(Statistics));
                }
            }
        }

        private ICommand _endTurnCommand;
        public ICommand EndTurnCommand
        {
            get => _endTurnCommand;
            set
            {
                if(_endTurnCommand != value)
                {
                    _endTurnCommand = value;
                    OnPropertyChanged(nameof(EndTurnCommand));
                }
            }
        }

        public GameVM()
        {
            boardVM = new BoardVM();
            _gameService = new GameService(boardVM.Pieces);
           
            // Initialize commands
            NewGameCommand = new RelayCommand(_ => NewGame());
            SaveGameCommand = new RelayCommand(_ => SaveGame());
            LoadGameCommand = new RelayCommand(_ => LoadGame());
            StatisticsCommand = new RelayCommand(_ => StatisticsGame());
            EndTurnCommand = new RelayCommand(_ => EndTurn());
            //MakeMoveCommand = new RelayCommand(_ => MakeMove()); // Assuming move requires parameters

            boardVM.PropertyChanged += DestinationTile_PropertyChanged;
        }


        private void DestinationTile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BoardVM.DestinationTile))
            {
                _gameService.MovePiece(boardVM.SelectedPiece, boardVM.DestinationTile);
                OnPropertyChanged(nameof(CurrentPlayer));
                OnPropertyChanged(nameof(PlayerOnePieces));
                OnPropertyChanged(nameof(PlayerTwoPieces));
                OnPropertyChanged(nameof(boardVM.myVMBoard));
            }
        }


        private void NewGame()
        {
            _gameService.CurrentTurn = Player.PlayerOne;
            boardVM = new BoardVM();
            _gameService.Pieces = boardVM.Pieces;
            boardVM.PropertyChanged += DestinationTile_PropertyChanged;

            OnPropertyChanged(nameof(CurrentPlayer));
            OnPropertyChanged(nameof(boardVM));
            // Update any other relevant properties or perform additional tasks
        }

        private void SaveGame()
        {
            _gameService.SaveGame();
            
            // Additional logic if needed
        }

        private void LoadGame()
        {
            _gameService.LoadGame();

           
            boardVM.Pieces = _gameService.Pieces;
            AllowMultipleJumps = _gameService.AllowMultipleJumps;
            // CurrentPlayer = _gameService.CurrentTurn;

            _gameService.PlayerOnePieces = 0;
            _gameService.PlayerTwoPieces = 0;
            foreach (var piece in boardVM.Pieces)
            {
                piece.CurrentTile = boardVM.myVMBoard[piece.Row * 8 + piece.Column];
                piece.CurrentTile.Piece = piece;
                piece.CurrentTile.IsOccupied = true;
                if (piece.Player == Player.PlayerOne)
                {
                    _gameService.PlayerOnePieces++;
                }
                else if (piece.Player == Player.PlayerTwo)
                {
                    _gameService.PlayerTwoPieces++;
                }
            }


            

            OnPropertyChanged(nameof(boardVM));
            OnPropertyChanged(nameof(CurrentPlayer));

        }

        private void StatisticsGame()
        {
           Statistics = _gameService.ShowStatistics();
        }

        private void EndTurn()
        {
            _gameService.EndTurn = true;
            boardVM.SelectedPiece.alreadyJumped= false;
            _gameService.SwitchTurns();
            OnPropertyChanged(nameof(CurrentPlayer));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public void UpdateGameState()
        //{
        //    // Update the game state here
        //    // Example:
        //    // GameState = newGameState;

        //    // Notify subscribers that the game state has changed
        //    OnPropertyChanged(nameof(_boardVM));
        //}
    }
}
