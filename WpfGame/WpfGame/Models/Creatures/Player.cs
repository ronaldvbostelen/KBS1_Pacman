using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Renderer;
using WpfGame.Controllers.Views;
using WpfGame.Generals;
using WpfGame.Properties;
using WpfGame.Views;

namespace WpfGame.Controllers
{
    public class Player : Sprite
    {

        public Player(double imageWidth, double imageHeight, double x, double y) 
            : base(x, y, true)
        {
            Image =  new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png")),
                Width = imageWidth, Height = imageHeight
            };
        }
    }
}
