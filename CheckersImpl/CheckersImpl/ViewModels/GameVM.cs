using System.ComponentModel;
using System.Windows.Input;
using CheckersImpl.Services;
using CheckersImpl.Commands;

namespace CheckersImpl.ViewModels
{
    public class GameVM : INotifyPropertyChanged
    {
        private GameService _gameService;
        public BoardVM boardVM { get; set; }

        // Example property bound to by the view
        public Player CurrentPlayer
        {
            get => _gameService.CurrentTurn;
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
            SaveGameCommand = new SaveGameCommand(SaveGame);
            LoadGameCommand = new RelayCommand(_ => LoadGame());
            //MakeMoveCommand = new RelayCommand(_ => MakeMove()); // Assuming move requires parameters
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

            OnPropertyChanged(nameof(boardVM)); // Notify that boardVM itself might have new data.
            boardVM.OnPropertyChanged(nameof(boardVM.Pieces));
            boardVM.OnPropertyChanged(nameof(boardVM.myVMBoard));
            
            
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
