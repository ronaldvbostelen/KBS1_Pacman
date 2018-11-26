using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGame.Models
{
    public class ObstacleEdit
    {
        public Ellipse Ellipse { get; set; }
        public double X { get; set; }
        public double Y { get; set; }


        public ObstacleEdit(double width, double height, double x, double y)
        {
            Ellipse = new Ellipse { Stroke = Brushes.Black, Fill = Brushes.White, Width = width, Height = height };
            X = x;
            Y = y;
        }
    }
}