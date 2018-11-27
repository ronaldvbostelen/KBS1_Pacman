using System.Windows.Media;

namespace WpfGame.Models
{
    public class Obstacle : PlaygroundObject
    {
        public bool IsEnabled { get; set; }

        public Obstacle(double x, double y, double width, double height) 
            : base(x, y)
        {
            Rectangle.RadiusX = Rectangle.RadiusY = 50;
            Rectangle.Width = width;
            Rectangle.Height = height;
            Rectangle.Stroke = Brushes.Black;
            Rectangle.Fill = Brushes.White;
            IsEnabled = false;
        }
    }
}