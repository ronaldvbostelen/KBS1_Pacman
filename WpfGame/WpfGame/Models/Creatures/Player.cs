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
using WpfGame.Properties;
using WpfGame.Views;

namespace WpfGame.Controllers
{
    public class Player : Sprite
    {
        public double Y { get; set; }
        public double X { get; set; }
        public Image PlayerImage;

        public Player(double imageWidth, double imageHeight, double x, double y) 
            : base(true)
        {
            X = x;
            Y = y;
            PlayerImage =  new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png")),
                Width = imageWidth, Height = imageHeight};
            

        }
    }
}
