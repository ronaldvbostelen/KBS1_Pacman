namespace WpfGame.Models
{
    public class TileMockup
    {
            public bool IsWall { get; }
            public bool HasCoin { get; }
            public bool HasObstacle { get; }
            public bool IsStart { get; }
            public bool IsEnd { get; }

            public TileMockup(bool isWall, bool hasCoin, bool hasObstacle, bool isStart, bool isEnd)
            {
                IsWall = isWall;
                HasCoin = hasCoin;
                HasObstacle = hasObstacle;
                IsStart = isStart;
                IsEnd = isEnd;
            }
        }
    }
