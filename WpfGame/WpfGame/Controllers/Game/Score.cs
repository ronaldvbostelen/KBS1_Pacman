using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfGame.Controllers
{
    class Score
    {
        public int ScoreValue { get; set; }

        public void WriteTotalScoreToHighscores()
        {
            // File to write to
            string path = $"{Environment.CurrentDirectory}\\Highscores.txt";
            string fileContent = $"{Settings.Default.Username} {ScoreValue.ToString()} {Environment.NewLine}";

            using (StreamWriter file = new StreamWriter(path, true))
            {
                file.WriteLine(fileContent);
            }
        }
    }
}
