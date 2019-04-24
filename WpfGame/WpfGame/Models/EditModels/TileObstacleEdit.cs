namespace WpfGame.Models.EditModels
{
    public class TileObstacleEdit
    {
        public TileEdit TileEdit { get; }
        public ObstacleEdit ObstacleEdit { get; }

        public TileObstacleEdit(TileEdit tileEdit, ObstacleEdit obstacleEdit)
        {
            TileEdit = tileEdit;
            ObstacleEdit = obstacleEdit;
        }
    }
}