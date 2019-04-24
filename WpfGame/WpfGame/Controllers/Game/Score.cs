using System;
using System.IO;

namespace WpfGame.Controllers.Game
{
    class Score
    {
        public int ScoreValue { get; set; }

        public void WriteTotalScoreToHighscores()
        {
            // File to write to
            string path = $"{Environment.CurrentDirectory}\\Highscores.txt";
            string fileContent = $"{Settings.Default.Username} {ScoreValue.ToString()}";

            using (StreamWriter file = new StreamWriter(path, true))
            {
                file.WriteLine(fileContent);
            }
        }
    }
}
