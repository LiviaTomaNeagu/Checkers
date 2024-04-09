using System.ComponentModel;
using System.Windows.Input;
using CheckersImpl.Services;
using CheckersImpl.Commands;

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
        public ICommand MakeMoveCommand { get; private set; }

        public GameVM()
        {
            boardVM = new BoardVM();
            _gameService = new GameService(boardVM.Pieces);
           
            // Initialize commands
            NewGameCommand = new RelayCommand(_ => NewGame());
            SaveGameCommand = new RelayCommand(_ => SaveGame());
            LoadGameCommand = new RelayCommand(_ => LoadGame());
            //MakeMoveCommand = new RelayCommand(_ => MakeMove()); // Assuming move requires parameters

            boardVM.PropertyChanged += DestinationTile_PropertyChanged;
        }

        private void DestinationTile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BoardVM.DestinationTile))
            {
                _gameService.MovePiece(boardVM.SelectedPiece, boardVM.DestinationTile);
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }


        private void NewGame()
        {
            _gameService.StartNewGame();
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
            foreach (var piece in boardVM.Pieces)
            {
                piece.CurrentTile = boardVM.myVMBoard[piece.Row * 8 + piece.Column];
                piece.CurrentTile.Piece = piece;
            }
            OnPropertyChanged(nameof(boardVM));
            OnPropertyChanged(nameof(CurrentPlayer));

        }

        private void MakeMove(object parameter)
        {
            // Convert parameter to your move type and call the corresponding method in GameService
            // _gameService.MakeMove(move);
            OnPropertyChanged(nameof(CurrentPlayer));
            // Update relevant properties
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
