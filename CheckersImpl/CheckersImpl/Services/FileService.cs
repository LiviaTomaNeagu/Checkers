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
using System.Collections.ObjectModel;

namespace CheckersImpl.Services
{
    public class FileService
    {
       
        public void SaveGame(ObservableCollection<PieceModel> pieceModels, Player currentPlayer, bool allowMultipleJumps)
        {
            var saveData = new
            {
                Pieces = pieceModels,
                CurrentTurn = currentPlayer.ToString(),
                AllowMultipleJumps = allowMultipleJumps
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
                        // Create pieces from loaded data
                        ObservableCollection<PieceModel> piecesModel = loadedData.Pieces;
                        // Parse the current player from loaded data
                        Player currentPlayer = Enum.TryParse<Player>(loadedData.CurrentTurn, out var parsedPlayer) ? parsedPlayer : Player.None;
                        // Extract the AllowMultipleJumps flag from loaded data
                        bool allowMultipleJumps = loadedData.AllowMultipleJumps;

                        // Assuming GameLoadResult can accept a boolean for allowMultipleJumps
                        return new GameLoadResult(piecesModel, currentPlayer, allowMultipleJumps);
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
            public ObservableCollection<PieceModel> PieceModel { get; set; }
            public Player CurrentPlayer { get; set; }
            public bool AllowMultipleJumps { get; set; }

            public GameLoadResult(ObservableCollection<PieceModel> pieceModel, Player currentPlayer, bool allowMultipleJumps)
            {
                PieceModel = pieceModel;
                CurrentPlayer = currentPlayer;
                AllowMultipleJumps = allowMultipleJumps;
            }
        }

        public class SaveData
        {
            public ObservableCollection<PieceModel> Pieces { get; set; }
            public string CurrentTurn { get; set; }
            public bool AllowMultipleJumps { get; set; }
        }

}
