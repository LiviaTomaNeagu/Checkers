using CheckersImpl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CheckersImpl.ViewModels
{
    public class BoardVM : INotifyPropertyChanged
    {
        public BoardModel BoardModel { get; }

        private ObservableCollection<TileModel> _myVMBoard;
        public ObservableCollection<TileModel> myVMBoard
        {
            get => _myVMBoard;
            set
            {
                if (_myVMBoard != value)
                {
                    _myVMBoard = value;
                    OnPropertyChanged(nameof(myVMBoard));
                }
            }
        }

        private ObservableCollection<PieceModel> _pieces;
        public ObservableCollection<PieceModel> Pieces
        {
            get => _pieces;
            set
            {
                if (_pieces != value)
                {
                    _pieces = value;
                    OnPropertyChanged(nameof(Pieces));
                }
            }
        }


        public BoardVM()
        {
            // Create an instance of BoardModel
            BoardModel = new BoardModel();

            // Initialize the view model's board based on the BoardModel
            InitializeBoard();

            Pieces.CollectionChanged += Pieces_CollectionChanged;
        }

        private void Pieces_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Handle different types of changes
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Handle added pieces
                    foreach (PieceModel newPiece in e.NewItems)
                    {
                        var tile = myVMBoard.FirstOrDefault(t => t.Row == newPiece.Row && t.Column == newPiece.Column);
                        if (tile != null)
                        {
                            tile.Piece = newPiece;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // Handle removed pieces
                    foreach (PieceModel oldPiece in e.OldItems)
                    {
                        var tile = myVMBoard.FirstOrDefault(t => t.Piece == oldPiece);
                        if (tile != null)
                        {
                            tile.Piece = null; // Or however you represent an empty tile
                        }
                    }
                    break;
                    // Handle other cases like Replace, Reset, etc., as needed
            }
        }

        private void InitializeBoard()
        {
            // Initialize the ObservableCollection
            myVMBoard = new ObservableCollection<TileModel>();

            // Loop through each cell in the BoardModel and create a TileModel for each one
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    // Get the tile from the BoardModel
                    var tile = BoardModel.myBoard[row, column];

                    // Add the tile to the ObservableCollection
                    myVMBoard.Add(tile);
                }
            }

            // Assuming Pieces is meant to be a flat list of all pieces, irrespective of position
            Pieces = new ObservableCollection<PieceModel>(BoardModel.myPieces);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
