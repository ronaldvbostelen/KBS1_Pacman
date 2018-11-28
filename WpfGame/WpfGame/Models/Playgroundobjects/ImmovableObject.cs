using System.Windows.Controls;
using System.Windows.Media;
using WpfGame.Generals;

namespace WpfGame.Models
{
    public class ImmovableObject: IPlaygroundObject, IState
    {
        public ObjectType ObjectType { get; set; }
        public Image Image { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool State { get; set; }

        public ImmovableObject(ObjectType objectType, Image image, double width, double height, double x, double y, bool state)
        {
            ObjectType = objectType;
            Image = image;
            Image.Width = width;
            Image.Height = height;
            Image.Stretch = Stretch.Fill;
            X = x;
            Y = y;
            State = state;
        }
    }
}