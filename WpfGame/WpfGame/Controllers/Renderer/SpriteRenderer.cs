using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfGame.Controllers.Behaviour;

namespace WpfGame.Controllers.Renderer
{
    static class SpriteRenderer
    {
        public static void Draw(UIElement element, Position position, Behaviour.Size size, Canvas canvas)
        {
            Canvas.SetLeft(element, position.Left - (size.Width / 2));
            Canvas.SetTop(element, position.Top - (size.Height / 2));
            canvas.Children.Add(element);
        }
    }
}
