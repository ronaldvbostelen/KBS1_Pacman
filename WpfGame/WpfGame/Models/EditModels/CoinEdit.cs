using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGame.Models.EditModels
{
    public class CoinEdit
    {
        public Ellipse Ellipse { get; }
        public double X { get; }
        public double Y { get; }


        public CoinEdit(double width, double height, double x, double y)
        {
            Ellipse = new Ellipse { Stroke = Brushes.Black, Fill = Brushes.Gold, Width = width, Height = height };
            X = x;
            Y = y;
        }
    }
}