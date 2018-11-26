using System.Windows.Media;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfGame.Models
{
    public class Coin : PlaygroundObject
    {
        public bool IsEnabled { get; set; }

        public Coin(double x, double y, double width, double height) 
            : base(x, y)
        {
            Rectangle.RadiusX = Rectangle.RadiusY = 50;
            Rectangle.Width = width;
            Rectangle.Height = height;
            Rectangle.Stroke = Brushes.Black;
            Rectangle.Fill = Brushes.White;
            //SpriteRenderer.Draw(100, 100, new Size(18, 18), new Image { Source = new BitmapImage(new Uri(@"\Assets\Sprites\Objects\coin.png", UriKind.Relative)) });
            IsEnabled = false;
        }
    }
}