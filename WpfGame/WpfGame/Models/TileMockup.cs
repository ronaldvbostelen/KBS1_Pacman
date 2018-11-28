namespace WpfGame.Models
{
    public class TileMockup
    {
        public bool IsWall { get; }
        public bool HasCoin { get; }
        public bool HasObstacle { get; }
        public bool IsStart { get; }
        public bool IsEnd { get; }
        public bool IsSpawn { get; set; }


        public TileMockup(bool isWall, bool hasCoin, bool hasObstacle, bool isStart, bool isEnd, bool isSpawn)
        {
            IsWall = isWall;
            HasCoin = hasCoin;
            HasObstacle = hasObstacle;
            IsStart = isStart;
            IsEnd = isEnd;
            IsSpawn = isSpawn;
        }
    }
}