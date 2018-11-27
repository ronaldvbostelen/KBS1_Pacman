using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGame.Models
{
    public class Tile : PlaygroundObject
    {
        public bool IsWall { get; set; }
        public bool HasCoin { get; set; }
        public bool HasObstacle { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }

        public Tile(double width, double height, double y, double x, bool isWall, bool hasCoin, bool hasObstacle, bool isStart, bool isEnd) : base(x, y)
        {
            Rectangle.Width = width;
            Rectangle.Height = height;
            Rectangle.Fill = isWall ? Brushes.Black : Brushes.Green;
            IsWall = isWall;
            HasCoin = hasCoin;
            HasObstacle = hasObstacle;
            IsStart = isStart;
            IsEnd = isEnd;
            if (isEnd)
            {
                Rectangle.Fill = Brushes.Red;
            }
        }
    }
}