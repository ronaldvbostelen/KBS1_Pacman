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
    static class ControlRenderer
    {
        public static void Draw(Position position, TextBlock control) //ToDo: this should be variable. For example, this could also be a TextBox.
        {
            Canvas.SetLeft(control, position.Left - (control.Width / 2));
            Canvas.SetTop(control, position.Top - (control.Height / 2));
            GameViewController.Canvas.Children.Add(control);
        }
    }
}
