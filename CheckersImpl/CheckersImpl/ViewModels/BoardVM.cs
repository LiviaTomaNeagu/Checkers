using CheckersImpl.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CheckersImpl.ViewModels
{
    internal class BoardVM
    {
        public ObservableCollection<TileModel> Board { get; set; }

        public BoardVM()
        {
            Board = new ObservableCollection<TileModel>();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    string color;
                    if (row % 2 == 0)
                    {
                        color = (column % 2 == 0) ? "lightbrown" : "darkbrown";
                    }
                    else
                    {
                        color = (column % 2 == 0) ? "darkbrown" : "lightbrown";
                    }
                    Board.Add(new TileModel(row, column, color));
                }
            }
        }

    }
}
