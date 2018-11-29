using System.Windows.Controls;
using System.Windows.Media;
using WpfGame.Generals;

namespace WpfGame.Models
{
    public class MovableObject : IMovable
    {
        public ObjectType ObjectType { get; set; }
        public Image Image { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Move CurrentMove { get; set; }
        public Move NextMove { get; set; }

        public MovableObject(ObjectType objectType, Image image, double width, double height, double x, double y)
        {
            ObjectType = objectType;
            Image = image;
            Image.Width = width;
            Image.Height = height;
            Image.Stretch = Stretch.Fill;
            X = x;
            Y = y;
            NextMove = CurrentMove = Move.Stop;
        }
    }
}