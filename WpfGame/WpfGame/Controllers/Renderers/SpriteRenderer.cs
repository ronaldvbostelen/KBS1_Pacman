using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Views;
using WpfGame.Views;

namespace WpfGame.Controllers.Renderer
{
    static class SpriteRenderer
    {
        private static Image spriteImage;
        
        public static void Draw(double x, double y, Behaviour.Size size, Image image)
        {
            spriteImage = image;
            spriteImage.Tag = "Player";
            
            Canvas.SetLeft(spriteImage, x);
            Canvas.SetTop(spriteImage, y);
            GameViewController.Canvas.Children.Add(spriteImage);
        }

        public static Image GetSpriteImage(string uri)
        {
            spriteImage = new Image();

            spriteImage.Source = new BitmapImage(new Uri(uri, UriKind.Relative));
            spriteImage.Height = 18;
            spriteImage.Width = 18;

            return spriteImage;
        }
        
    }
}
