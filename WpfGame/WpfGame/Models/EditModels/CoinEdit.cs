using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGame.Models
{
    public class CoinEdit
    {
        public Ellipse Ellipse { get; set; }
        public double X { get; set; }
        public double Y { get; set; }


        public CoinEdit(double width, double height, double x, double y)
        {
            Ellipse = new Ellipse { Stroke = Brushes.Black, Fill = Brushes.Gold, Width = width, Height = height };
            X = x;
            Y = y;
        }
    }
}