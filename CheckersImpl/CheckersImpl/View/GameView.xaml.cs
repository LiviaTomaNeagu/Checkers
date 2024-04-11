using CheckersImpl.Services;
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
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
            //DataContext = new GameVM();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }



        private void MenuAllow_Click(object sender, RoutedEventArgs e)
        {

            if (AllowTextBox.Visibility == Visibility.Visible)
            {
                AllowTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                AllowTextBox.Visibility = Visibility.Visible;
            }

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is the checkers GAME!\n\n" +
                "Checkers, also known as draughts, is a classic board game played by two players on an 8x8 checkered gameboard. Each player starts with 12 pieces, usually distinguishable by color, placed on the dark squares of the three rows closest to them. The objective of the game is to capture all of the opponent's pieces or block them so they cannot move.\r\n\r\nPlayers take turns moving their pieces diagonally forward, one square at a time, to an adjacent empty square. If a player's piece reaches the opposite end of the board, it is typically \"crowned\" and becomes a \"king,\" allowing it to move both forwards and backwards.\r\n\r\nThe main mechanics of the game involve capturing the opponent's pieces by jumping over them diagonally if an adjacent square is empty. Multiple jumps can be made in a single turn if consecutive capturing moves are possible. Strategic thinking, anticipation, and planning are crucial for success in checkers, making it a timeless and engaging game for players of all ages.");
        }
    }
}
