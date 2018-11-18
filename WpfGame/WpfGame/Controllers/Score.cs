using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfGame.Controllers
{
    static class Score
    {
        private static int scoreValue = 0;

        private static int ScoreValue
        {
            get
            {
                return scoreValue;
            }
            set
            {
                scoreValue = ScoreValue;
            }
        }

        public static TextBlock DrawScore()
        {
            TextBlock score = new TextBlock();

            score.Height = 50;
            score.Width = 100;
            score.Visibility = Visibility.Visible;
            score.FontSize = 20;
            score.Foreground = new SolidColorBrush(Colors.Black);
            score.Text = ScoreValue.ToString();

            return score;
        }
    }
}
