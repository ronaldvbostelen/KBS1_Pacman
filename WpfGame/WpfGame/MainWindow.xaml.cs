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
using WpfGame.Controllers;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Renderer;

namespace WpfGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas canvas;

        public MainWindow()
        {
            InitializeComponent();
            canvas = GameCanvas;

            Player player = new Player();
            player.Draw();

            Enemy enemy = new Enemy(100,100);
            enemy.Draw();

            Enemy enemy2 = new Enemy(200,200);
            enemy2.Draw();

            Clock clock = new Clock();
            clock.Initialize();

            //var score = Score.DrawScore();

            //Canvas.SetTop(score, 0);
            //Canvas.SetRight(score, 0);
            //GameCanvas.Children.Add(score);
        }
    }
}
