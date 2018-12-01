using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Renderer
{
    public class EnemyFactory
    {
        private GameValues _gameValues;
        private BitmapImage _enemysFirstFace;
        private double enemyWidth, enemyHeight, enemyStartPointCorrection;

        public void LoadFactory(GameValues gameValues)
        {
            _gameValues = gameValues;

            _enemysFirstFace = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Enemy/blinky-right.png"));
            enemyWidth = _gameValues.TileWidth * 0.93;
            enemyHeight = _gameValues.TileHeight * 0.93;
            enemyStartPointCorrection = 1.003;
        }
        
        public MovableObject LoadEnemy(List<IPlaygroundObject> list)
        {
            try
            {
                var spawnpoint = list.First(x => x.ObjectType == ObjectType.SpawnPoint);
                return new MovableObject(ObjectType.Enemy, new Image { Source = _enemysFirstFace }, enemyWidth, enemyHeight, spawnpoint.X * enemyStartPointCorrection, spawnpoint.Y * enemyStartPointCorrection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentNullException("woeeeps", "your playboards doesn't contain a spawnpoint");
            }
        }

        public void DrawEnemy(MovableObject enemy, Canvas canvas)
        {
            Canvas.SetTop(enemy.Image, enemy.Y);
            Canvas.SetLeft(enemy.Image, enemy.X);
            canvas.Children.Add(enemy.Image);
        }

    }
}