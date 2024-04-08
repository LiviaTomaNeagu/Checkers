using CheckersImpl.Services;
using CheckersImpl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CheckersImpl.View
{
    public partial class BoardView : UserControl
    {
        private PieceModel selectedPiece;

        public BoardView()
        {
            InitializeComponent();
            DataContext = new BoardVM();

            // Subscribe to the MouseLeftButtonDown event for Ellipse elements representing pieces
            foreach (UIElement child in LogicalTreeHelper.GetChildren(this).OfType<UIElement>())
            {
                if (child is Ellipse)
                {
                    child.MouseLeftButtonDown += SelectPiece;
                }

                else if (child is Rectangle)
                {
                    child.MouseLeftButtonDown += SelectTile;
                }
            }

            //MouseLeftButtonDown += SelectTile;
        }




        //private void PieceWasSelected(object sender, MouseButtonEventArgs e)
        //{
        //    // Get the clicked UI element
        //    var clickedElement = e.OriginalSource as FrameworkElement;

        //    // Check if the clicked element is a piece
        //    if (clickedElement.DataContext is PieceModel)
        //    {
        //        // Handle the click on the piece
        //        var newlySelectedPiece = (PieceModel)clickedElement.DataContext;

        //        if (selectedPiece != null && selectedPiece != newlySelectedPiece)
        //        {
        //            // Deselect the previously selected piece
        //            selectedPiece.IsSelected = false;
        //        }

        //        // Update the selected piece and tile
        //        selectedPiece = newlySelectedPiece;
        //        selectedTile = selectedPiece.CurrentTile; // Update this based on your PieceModel implementation

        //        // Select the newly clicked piece
        //        selectedPiece.IsSelected = true;
        //    }
        //}

        //private void TileWasSelected(object sender, MouseButtonEventArgs e)
        //{
        //    // Get the clicked UI element
        //    var clickedElement = e.OriginalSource as FrameworkElement;

        //    // Check if the clicked element is a tile
        //    if (clickedElement.DataContext is TileModel)
        //    {
        //        // Handle the click on the tile
        //        var destinationTile = (TileModel)clickedElement.DataContext;

        //        // Ensure that a piece is selected and a destination tile is chosen
        //        if (selectedPiece != null && destinationTile != null)
        //        {
        //            // Move the piece to the destination tile
        //            GameService gameService = new GameService();
        //            gameService.MovePiece(selectedPiece, selectedPiece.CurrentTile, destinationTile);

        //            // After moving the piece, reset selectedPiece and selectedTile to null
        //            selectedPiece = null;
        //            selectedTile = null;
        //        }
        //    }
        //}

        private void SelectPiece(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked UI element
            var clickedElement = e.OriginalSource as FrameworkElement;

            // Check if the clicked element is a piece
            if (clickedElement.DataContext is PieceModel)
            {
                // Handle the click on the piece
                var newlySelectedPiece = (PieceModel)clickedElement.DataContext;

                // Deselect the previously selected piece (if any)
                if (selectedPiece != null && selectedPiece != newlySelectedPiece)
                {
                    selectedPiece.IsSelected = false;
                }

                // Update the selected piece
                selectedPiece = newlySelectedPiece;

                // Select the newly clicked piece
                selectedPiece.IsSelected = true;
            }
        }

        private void SelectTile(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked UI element
            var clickedElement = e.OriginalSource as FrameworkElement;

            // Check if the clicked element is a tile
            if (clickedElement.DataContext is TileModel)
            {
                // Handle the click on the tile
                var destinationTile = (TileModel)clickedElement.DataContext;

                // Ensure that a piece is selected and a destination tile is chosen
                if (selectedPiece != null && destinationTile != null)
                {
                    // Move the piece to the destination tile
                    GameService gameService = new GameService(((BoardVM)DataContext).Pieces);
                    gameService.MovePiece(selectedPiece, selectedPiece.CurrentTile, destinationTile);

                    // After moving the piece, reset selectedPiece to null
                    selectedPiece.IsSelected = false;
                    selectedPiece = null;
                }
            }
        }

    }

}
