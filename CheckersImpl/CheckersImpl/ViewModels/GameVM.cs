using System.ComponentModel;
using System.Windows.Input;
using CheckersImpl.Services;
using CheckersImpl.Commands;

namespace CheckersImpl.ViewModels
{
    public class GameVM : INotifyPropertyChanged
    {
        private GameService _gameService;

        // Example property bound to by the view
        public Player CurrentPlayer
        {
            get => _gameService.CurrentTurn;
        }

        // Commands
        public ICommand NewGameCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }
        public ICommand LoadGameCommand { get; private set; }
        public ICommand MakeMoveCommand { get; private set; }

        public GameVM()
        {
            _gameService = new GameService();

            // Initialize commands
            NewGameCommand = new RelayCommand(_ => NewGame());
            SaveGameCommand = new SaveGameCommand(SaveGame);
            LoadGameCommand = new LoadGameCommand(LoadGame);
            //MakeMoveCommand = new RelayCommand(_ => MakeMove()); // Assuming move requires parameters
        }

        private void NewGame()
        {
            _gameService.StartNewGame();
            OnPropertyChanged(nameof(CurrentPlayer));
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
            OnPropertyChanged(nameof(CurrentPlayer));
            // Update any other relevant properties based on the loaded game state
        }

        private void MakeMove(object parameter)
        {
            // Convert parameter to your move type and call the corresponding method in GameService
            // _gameService.MakeMove(move);
            OnPropertyChanged(nameof(CurrentPlayer));
            // Update relevant properties
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
