using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGame.Models.EditModels
{
    public class ObstacleEdit
    {
        public Ellipse Ellipse { get; }
        public double X { get; }
        public double Y { get; }


        public ObstacleEdit(double width, double height, double x, double y)
        {
            Ellipse = new Ellipse { Stroke = Brushes.Black, Fill = Brushes.White, Width = width, Height = height };
            X = x;
            Y = y;
        }
    }
}