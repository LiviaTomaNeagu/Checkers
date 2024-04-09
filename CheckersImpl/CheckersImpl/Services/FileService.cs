using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Reflection;

namespace CheckersImpl.Services
{
    public class FileService
    {
        private Color ConvertStringToColor(string colorString)
        {
            // First, try to use a known color name
            var colorProperty = typeof(Colors).GetProperty(colorString, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (colorProperty != null)
            {
                return (Color)colorProperty.GetValue(null);
            }

            // If not a known name, try to parse a hexadecimal color value
            try
            {
                // Add handling for shorthand hex codes (e.g., "#FFF")
                if (colorString.StartsWith("#") && colorString.Length == 4)
                {
                    string r = colorString.Substring(1, 1);
                    string g = colorString.Substring(2, 1);
                    string b = colorString.Substring(3, 1);
                    colorString = $"#{r}{r}{g}{g}{b}{b}";
                }

                return (Color)ColorConverter.ConvertFromString(colorString);
            }
            catch
            {
                // Return a default color if parsing fails
                return Colors.Transparent;
            }
        }
        public void SaveGame(BoardModel boardModel, Player currentPlayer)
        {
            var saveData = new
            {
                Board = GetBoardData(boardModel),
                CurrentTurn = currentPlayer.ToString()
            };

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                DefaultExt = "json",
                AddExtension = true,
                FileName = "CheckersGame"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string json = JsonConvert.SerializeObject(saveData, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private List<List<object>> GetBoardData(BoardModel boardModel)
        {
            var boardData = new List<List<object>>();
            for (int i = 0; i < boardModel.myBoard.GetLength(0); i++)
            {
                var rowData = new List<object>();
                for (int j = 0; j < boardModel.myBoard.GetLength(1); j++)
                {
                    var tile = boardModel.myBoard[i, j];
                    if (tile.Piece != null)
                    {
                        rowData.Add(new
                        {
                            PieceColor = tile.Piece.Color.ToString(),
                            IsPieceKing = tile.Piece.IsKing,
                            Position = new { Row = i, Column = j }
                        });
                    }
                    else
                    {
                        rowData.Add(null);
                    }
                }
                boardData.Add(rowData);
            }
            return boardData;
        }



        private PieceModel[] CreatePiecesFromLoadedData(List<List<TileData>> loadedBoard)
        {
            List<PieceModel> pieces = new List<PieceModel>();

            for (int row = 0; row < loadedBoard.Count; row++)
            {
                for (int column = 0; column < loadedBoard[row].Count; column++)
                {
                    var tile = loadedBoard[row][column];
                    if (tile != null && tile.PieceColor != null && tile.IsPieceVisible != null)
                    {
                        // Convert string color to System.Windows.Media.Color
                        var color = ConvertStringToColor(tile.PieceColor);

                        // Create and configure the PieceModel, including position
                        var piece = new PieceModel(new SolidColorBrush(color), tile.IsPieceKing, row, column);
                        pieces.Add(piece);
                    }
                }
            }

            return pieces.ToArray();
        }



        public GameLoadResult LoadGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                DefaultExt = "json",
                AddExtension = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    string json = File.ReadAllText(filePath);
                    // Create a class to match the structure of your save data


                    // Deserialize the JSON into the SaveData class
                    SaveData loadedData = JsonConvert.DeserializeObject<SaveData>(json);

                    if (loadedData != null)
                    {
                        PieceModel[] boardModel = CreatePiecesFromLoadedData(loadedData.Board);
                        Player currentPlayer = Enum.TryParse<Player>(loadedData.CurrentTurn, out var parsedPlayer) ? parsedPlayer : Player.None;

                        return new GameLoadResult(boardModel, currentPlayer);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Failed to read the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Failed to deserialize JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null; // Indicate that loading was not successful
        }



    }

        public class GameLoadResult
        {
            public PieceModel[] PieceModel { get; set; }
            public Player CurrentPlayer { get; set; }

            public GameLoadResult(PieceModel[] pieceModel, Player currentPlayer)
            {
                PieceModel = pieceModel;
                CurrentPlayer = currentPlayer;
            }
        }

        public class SaveData
        {
            public List<List<TileData>> Board { get; set; }
            public string CurrentTurn { get; set; }
        }

    public class TileData
    {
        public bool IsOccupied { get; set; }
        public string PieceColor { get; set; } // You might want to change the type to a more appropriate one
        public bool IsPieceKing { get; set; }
        public bool IsPieceVisible { get; set; }
        public Position Position { get; set; } // Define Position property

        // Constructor to initialize properties
        public TileData(int row, int column)
        {
            Position = new Position { Row = row, Column = column };
        }
    }

    // Define Position class to represent the row and column
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }



}
