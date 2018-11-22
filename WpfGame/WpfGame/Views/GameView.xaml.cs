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
using WpfGame.Controllers.Objects;

namespace WpfGame.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : Page
    {
        public static Canvas canvas;

        public GameView()
        {
            InitializeComponent();
            canvas = GameCanvas;

            Player player = new Player();

            Clock clock = new Clock();
            clock.Initialize();

            Coin coin = new Coin();
        }
    }
}
