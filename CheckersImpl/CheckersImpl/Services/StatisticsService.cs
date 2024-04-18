using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Documents;

namespace CheckersImpl.Services
{
    public class StatisticsService
    {
        private int firstPlayerWins;
        private int secondPlayerWins;
        private int draws;
        private int totalGames;
        public StatisticsService()
        {
            LoadStatisticsFromJson();
        }

        public void UpdateStatistics(Player PlayerName)
        {
            totalGames++;
            if (PlayerName == Player.PlayerOne)
            {
                firstPlayerWins++;
            }
            else if (PlayerName == Player.PlayerTwo)
            {
                secondPlayerWins++;
            }
            else
            {
                draws++;
            }

            SaveStatisticsToJson();
        }

        public string ShowStatistics()
        {
            // Calculate the percentages
            double firstPlayerWinPercentage = (totalGames > 0) ? (double)firstPlayerWins / totalGames * 100 : 0;
            double secondPlayerWinPercentage = (totalGames > 0) ? (double)secondPlayerWins / totalGames * 100 : 0;
            double drawPercentage = (totalGames > 0) ? (double)draws / totalGames * 100 : 0;

            // Format the output string
            StringBuilder stats = new StringBuilder();
            stats.AppendLine("Statistics:");
            stats.AppendLine($"Total games played: {totalGames}");
            stats.AppendLine($"Player One Wins: {firstPlayerWins} ({firstPlayerWinPercentage:N2}%)");
            stats.AppendLine($"Player Two Wins: {secondPlayerWins} ({secondPlayerWinPercentage:N2}%)");
            stats.AppendLine($"Draws: {draws} ({drawPercentage:N2}%)");

            return stats.ToString();
        }

        private void SaveStatisticsToJson()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(appDirectory, "../../statistics.json");
            string filePath = Path.GetFullPath(relativePath);
            // Get the path to the statistics.json file

            var statistics = new
            {
                FirstPlayerWins = firstPlayerWins,
                SecondPlayerWins = secondPlayerWins,
                Draws = draws,
                TotalGames = totalGames
            };

            string json = JsonConvert.SerializeObject(statistics, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        private void LoadStatisticsFromJson()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(appDirectory, "../../statistics.json");
            string filePath = Path.GetFullPath(relativePath);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var statistics = JsonConvert.DeserializeObject<StatisticsData>(json);

                if (statistics != null)
                {
                    firstPlayerWins = statistics.FirstPlayerWins;
                    secondPlayerWins = statistics.SecondPlayerWins;
                    draws = statistics.Draws;
                    totalGames = statistics.TotalGames;
                }
            }
        }
    }
    public class StatisticsData
    {
        public int FirstPlayerWins { get; set; }
        public int SecondPlayerWins { get; set; }
        public int Draws { get; set; }
        public int TotalGames { get; set; }
    }
}
