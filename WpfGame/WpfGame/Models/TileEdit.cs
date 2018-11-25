using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGame.Models
{
    public class TileEdit
    {
        public Rectangle Rectangle { get; set; }
        public double Y { get; set; }
        public double X { get; set; }
        public bool IsWall { get; set; }
        public bool HasCoin { get; set; }
        public bool HasObstacle { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }

        public TileEdit(double width, double height, double y, double x, bool isWall, bool hasCoin, bool hasObstacle, bool isStart, bool isEnd)
        {
            Rectangle = new Rectangle { Width = width, Height = height,Stroke = Brushes.Black};
            
            Rectangle.Fill = isWall ? Brushes.Black : Brushes.Green;

            Y = y;
            X = x;

            IsWall = isWall;
            HasCoin = hasCoin;
            HasObstacle = hasObstacle;
            IsStart = isStart;
            IsEnd = isEnd;
        }
    }
}
