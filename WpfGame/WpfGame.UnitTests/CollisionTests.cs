using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Behaviour;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;
using System.Windows.Controls;
using NUnit.Framework;

namespace WpfGame.UnitTests
{
    [TestFixture(Description = "CollisionTests")]
    public class CollisionTests
    {
        //   this is our fictive playground (everything is 50x50)
        //   --------------200-------------------------------->
        //   | x,y 5,5 player             x,y 150,5 obstactal (on)
        //   |  \                         \ 
        //   |   [_]                       [_]
        //   2
        //   0
        //   0  x,y 5, 105 coin (on)      x,y 150, 125 enemy     
        //   |  \                         \
        //   |   [_]                       [_] 
        //   |
        //   |  x,y 5, 200 endpoint      x,y 150, 200 wall
        //   |  \                        \
        //   v   [_]                      [_]
        //

        private GameValues _gameValues;
        private List<IPlaygroundObject> _playgroundObjects;
        private CollisionDetecter _collisionDetector;
        private MovableObject _player;
        private MovableObject _enemy;

        public CollisionTests()
        {
            if (Application.ResourceAssembly == null)
            {
                Application.ResourceAssembly = typeof(MainWindow).Assembly;
            }

            _gameValues = new GameValues();

            _gameValues.PlayCanvasHeight = 200;
            _gameValues.PlayCanvasWidth = 200;
            _gameValues.Movement = 2.5;

            _collisionDetector = new CollisionDetecter(_gameValues);
            
            _playgroundObjects = new List<IPlaygroundObject>
            {
                new MovableObject(ObjectType.Player,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,5,5),
                new ImmovableObject(ObjectType.Obstacle,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,150,5,true),
                new ImmovableObject(ObjectType.Coin,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,5,105,true),
                new MovableObject(ObjectType.Enemy,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,150,125),
                new StaticObject(ObjectType.EndPoint,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,5,200),
                new StaticObject(ObjectType.Wall,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,150,200)
            };

            //extract player and enemy
            _player = (MovableObject) _playgroundObjects.First(x => x.ObjectType == ObjectType.Player);
            _enemy = (MovableObject)_playgroundObjects.First(x => x.ObjectType == ObjectType.Enemy);
        }
        
        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerMovesInFreeArea_NoCollsion(Move move)
        {
            Assert.AreEqual(Collision.Clear, _collisionDetector.ObjectCollision(_playgroundObjects, _player, move));
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerMovesOnCoin_CoinEventIsFired(Move move)
        {
            //move player on the middle of the coin
            _player.X = 5 + 25;
            _player.Y = 105 + 25;

            //bool for cointeventcheck
            bool CoinEventIsFired = false;

            //subscribe to coinevent
            _collisionDetector.CoinCollision += _collisionDetector_CoinCollision;

            //move the player around
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);

            Assert.True(CoinEventIsFired);

            void _collisionDetector_CoinCollision(object sender, ImmovableEventArgs e)
            {
                CoinEventIsFired = true;
            }
            
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerMovesOnObstacle_ObstacleEventIsFired(Move move)
        {
            //move player on the middle of the obstacle
            _player.X = 150 + 25;
            _player.Y = 5 + 25;

            //bool for cointeventcheck
            bool ObstacleEventIsFired = false;

            //subscribe to coinevent
            _collisionDetector.ObstacleCollision += _collisionDetector_ObstacleCollision;

            //move the player around
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);

            Assert.True(ObstacleEventIsFired);

            void _collisionDetector_ObstacleCollision(object sender, EventArgs e)
            {
                ObstacleEventIsFired = true;
            }
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerMovesOnWall_PlayerMoveEqualsStop(Move move)
        {
            //move player on the middle of the wall
            _player.X = 150 + 25;
            _player.Y = 200 + 25;

            //bool for cointeventcheck
            bool ObstacleEventIsFired = false;

            //subscribe to coinevent
            _collisionDetector.ObstacleCollision += _collisionDetector_ObstacleCollision;

            //move the player around
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);

            Assert.True(ObstacleEventIsFired);

            void _collisionDetector_ObstacleCollision(object sender, EventArgs e)
            {
                ObstacleEventIsFired = true;
            }
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void EnemyMovesInFreeArea_NoCollsion(Move move)
        {
            Assert.AreEqual(Collision.Clear, _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move));
        }
    }
}