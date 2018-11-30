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
            // Create a file to write to
            string path = $"{Environment.CurrentDirectory}\\Highscores.txt";

            if(!File.Exists(path))
            {
                File.Create(path);
            }

            File.WriteAllText(path, ScoreValue.ToString());
        }
    }
}
