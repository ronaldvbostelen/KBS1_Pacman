using System.Windows.Shapes;

namespace WpfGame.Models
{
    public class TileObstacleEdit
    {
        public TileEdit TileEdit { get; set; }
        public ObstacleEdit ObstacleEdit { get; set; }

        public TileObstacleEdit(TileEdit tileEdit, ObstacleEdit obstacleEdit)
        {
            TileEdit = tileEdit;
            ObstacleEdit = obstacleEdit;
        }
    }
}