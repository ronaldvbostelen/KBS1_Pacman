using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Models.Playgroundobjects;
using WpfGame.Values;

namespace WpfGame.Controllers.Renderers
{
    public class EnemyFactory
    {
        private GameValues _gameValues;
        private BitmapImage _enemysFirstFace;
        private double _enemyWidth, _enemyHeight, _enemyStartPointCorrection;

        public void LoadFactory(GameValues gameValues)
        {
            _gameValues = gameValues;

            _enemysFirstFace = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Enemy/blinky-right.png"));
            _enemyWidth = _gameValues.TileWidth * 0.93;
            _enemyHeight = _gameValues.TileHeight * 0.93;
            _enemyStartPointCorrection = 1.003;
        }
        
        public MovableObject LoadEnemy(List<IPlaygroundObject> list)
        {
            try
            {
                var spawnpoint = list.First(x => x.ObjectType == ObjectType.SpawnPoint);
                return new MovableObject(ObjectType.Enemy, new Image { Source = _enemysFirstFace }, _enemyWidth, _enemyHeight, spawnpoint.X * _enemyStartPointCorrection, spawnpoint.Y * _enemyStartPointCorrection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentNullException("woeeeps", "your playboards doesn't contain a spawnpoint");
            }
        }

        public MovableObject DrawEnemy(MovableObject enemy, Canvas canvas)
        {
            Canvas.SetTop(enemy.Image, enemy.Y);
            Canvas.SetLeft(enemy.Image, enemy.X);
            canvas.Children.Add(enemy.Image);
            return enemy;
        }
    }
}