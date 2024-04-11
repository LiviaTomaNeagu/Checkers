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

        public BoardView()
        {
            InitializeComponent();

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


        private void SelectPiece(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked UI element
            var clickedElement = e.OriginalSource as FrameworkElement;

            // Check if the clicked element is a piece
            if (clickedElement.DataContext is PieceModel)
            {
                // Handle the click on the piece
                var newlySelectedPiece = (PieceModel)clickedElement.DataContext;
                if(((BoardVM)DataContext).DestinationTile != null && ((BoardVM)DataContext).DestinationTile.Piece != null && ((BoardVM)DataContext).DestinationTile.Piece!= newlySelectedPiece && ((BoardVM)DataContext).DestinationTile.Piece.alreadyJumped == true )
                {
                    MessageBox.Show("You must continue jumping with the same piece");
                }
                else
                {

                    // Deselect the previously selected piece (if any)
                    if (((BoardVM)DataContext).SelectedPiece != null && ((BoardVM)DataContext).SelectedPiece != newlySelectedPiece)
                    {
                        ((BoardVM)DataContext).SelectedPiece.IsSelected = false;
                    }

                    if (((BoardVM)DataContext).SelectedPiece != null && ((BoardVM)DataContext).SelectedPiece == newlySelectedPiece)
                    {
                        ((BoardVM)DataContext).SelectedPiece.IsSelected = false;
                        newlySelectedPiece = null;
                    }

                // Update the selected piece
                ((BoardVM)DataContext).SelectedPiece = newlySelectedPiece;

                    if (((BoardVM)DataContext).SelectedPiece != null)
                    {
                        // Select the newly clicked piece
                        ((BoardVM)DataContext).SelectedPiece.IsSelected = true;
                    }
                }

                
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
                if (((BoardVM)DataContext).SelectedPiece != null && destinationTile != null)
                {

                    ((BoardVM)DataContext).DestinationTile = destinationTile;

                    // After moving the piece, reset selectedPiece to null
                    ((BoardVM)DataContext).SelectedPiece.IsSelected = false;
                    ((BoardVM)DataContext).SelectedPiece = null;
                }
            }
        }

    }

}
