using System.Windows.Shapes;

namespace WpfGame.Models
{
    public abstract class PlaygroundObject
    {
        public Rectangle Rectangle { get; set; }
        public double Y { get; set; }
        public double X { get; set; }

        public PlaygroundObject(double x, double y)
        {
            Rectangle = new Rectangle();
            X = x;
            Y = y;
        }
    }
}
