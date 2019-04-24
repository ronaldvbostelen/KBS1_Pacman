using System.Windows.Controls;

namespace WpfGame.Generals
{
    public interface IPlaygroundObject
    {
        ObjectType ObjectType { get; set; }
        Image Image { get; set; }
        double X { get; set; }
        double Y { get; set; }
    }
}