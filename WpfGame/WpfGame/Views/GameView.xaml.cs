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
using System.Windows.Threading;
using WpfGame.Controllers;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;

namespace WpfGame.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : Page
    {
        public static Canvas Canvas;
        private Player _player;
        private double _y, _x;

        public GameView()
        {
            InitializeComponent();
            Canvas = GameCanvas;

            player = new Player();
            player.Draw();

            Clock clock = new Clock();
            clock.Initialize();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null) window.KeyDown += OnButtonKeyDown;
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    _y += 25;
                    Canvas.SetTop(player.Image, _y);
                    break;
                case Key.Up:
                    _y -= 25;
                    Canvas.SetTop(player.Image, _y);
                    break;
                case Key.Left:
                    _x -= 25;
                    Canvas.SetLeft(player.Image, _x);
                    break;
                case Key.Right:
                    _x += 25;
                    Canvas.SetLeft(player.Image, _x);
                    break;
            }
        }
    }
}
