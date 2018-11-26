namespace WpfGame.Models
{
    public class Coin : PlaygroundObject
    {
        public Coin(double x, double y) : base(x, y)
        {
            //to make a ellipse from rect
            Rectangle.RadiusX = Rectangle.RadiusX = 50;
        }
    }
}