using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
        //   --------------500-------------------------------->
        //   | x,y 50,50 player             x,y 250,50 obstactal (on)
        //   |  \                         \ 
        //   |   [_]                       [_]
        //   5
        //   0
        //   0  x,y 50, 200 coin (on)      x,y 250, 200 enemy     
        //   |  \                         \
        //   |   [_]                       [_] 
        //   |
        //   |  x,y 50, 350 endpoint      x,y 250, 350 wall
        //   |  \         _               \
        //   v   [_]     | [_]            [_]
        //               \
        //                x,y 150,350 spawnpoint   
        // 
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

            _gameValues.PlayCanvasHeight = 500;
            _gameValues.PlayCanvasWidth = 500;
            _gameValues.Movement = 2.5;

            _collisionDetector = new CollisionDetecter(_gameValues);
            
            _playgroundObjects = new List<IPlaygroundObject>
            {
                new MovableObject(ObjectType.Player,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,50,50),
                new ImmovableObject(ObjectType.Obstacle,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,250,50,true),
                new ImmovableObject(ObjectType.Coin,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,50,200,true),
                new MovableObject(ObjectType.Enemy,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,250,200),
                new StaticObject(ObjectType.EndPoint,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,50,350),
                new StaticObject(ObjectType.Wall,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,250,350),
                new StaticObject(ObjectType.SpawnPoint,new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                },50,50,150,350)
            };

            //extract player and enemy
            _player = (MovableObject) _playgroundObjects.Find(x => x.ObjectType == ObjectType.Player);
            _enemy = (MovableObject)_playgroundObjects.Find(x => x.ObjectType == ObjectType.Enemy);
        }
        
        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerMovesInFreeArea_NoCollsion(Move move)
        {
            //move player to a nice empty area
            _player.X = 100;
            _player.Y = 100;

            //test if the player can move free
            Assert.AreEqual(Collision.Clear, _collisionDetector.ObjectCollision(_playgroundObjects, _player, move));
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerMovesOnCoin_CoinEventIsFired(Move move)
        {
            //move player on the same x/y as the coin
            _player.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Coin).X;
            _player.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Coin).Y;

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
            //move player on the same x/y as the obstacle
            _player.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Obstacle).X;
            _player.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Obstacle).Y;

            //bool for obstacleeventcheck
            bool ObstacleEventIsFired = false;

            //subscribe to obstacleevent
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
            //move player on the same x/y as the wall
            _player.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Wall).X;
            _player.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Wall).Y;

            //simulate a move
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);
            
            Assert.True(_player.CurrentMove == Move.Stop);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerMovesAgainstBorder_PlayerMoveEqualsStop(Move move)
        {
            //move player next to the wall in the upper right corner
            _player.X = _gameValues.PlayCanvasWidth;
            _player.Y = 0;

            //simulate a move
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);

            Assert.True(_player.CurrentMove == Move.Stop);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerTouchesEndpoint_EndpointEventIsFired(Move move)
        {
            //move player on the same x/y as the endpoint
            _player.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.EndPoint).X;
            _player.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.EndPoint).Y;

            //bool for endgameeventcheck
            bool EndGameEvent = false;

            //subscribe to endgameevent
            _collisionDetector.EndpointCollision += (sender, args) => { EndGameEvent = true; };

            //we tweak the movement cause a >99% hit is required
            _gameValues.Movement = 0.1;
            
            //move the player around
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);

            Assert.True(EndGameEvent);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerHitsAEnemy_EnemyHitEventIsFired(Move move)
        {
            //move player on the same x/y as the obstacle
            _player.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Enemy).X;
            _player.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Enemy).Y;

            //bool for enemyeventcheck
            bool EnemyEventIsFired = false;

            //subscribe to enemyevent
            _collisionDetector.EnemyCollision += (sender, args) => { EnemyEventIsFired = true; };

            //move the player around
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);

            Assert.True(EnemyEventIsFired);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void PlayerHitsSpawnPoint_MoveEqualsStop(Move move)
        {
            //move player on the same x/y as the spawnpoint
            _player.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.SpawnPoint).X;
            _player.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.SpawnPoint).Y;

            //simulate a move
            _collisionDetector.ObjectCollision(_playgroundObjects, _player, move);

            Assert.True(_player.CurrentMove == Move.Stop);
        }

        ///
        /////////////////////////////////////////////////////////ENEMY TESTS //////////////////////////////////////////////////
        /// 
        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void EnemyMovesInFreeArea_NoCollsion(Move move)
        {           
            //move enemy to a nice empty area
            _enemy.X = 100;
            _enemy.Y = 100;

            //test if the enemy can move free
            Assert.AreEqual(Collision.Clear, _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move));
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void EnemyMovesOnCoin_CoinEventIsNotFired(Move move)
        {
            //move Enemy on the same x/y as the coin
            _enemy.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Coin).X;
            _enemy.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Coin).Y;

            //bool for cointeventcheck
            bool CoinEventIsFired = false;

            //subscribe to coinevent
            _collisionDetector.CoinCollision += _collisionDetector_CoinCollision;

            //move the Enemy around
            _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move);

            Assert.False(CoinEventIsFired);

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
        public void EnemyMovesOnObstacle_ObstacleEventIsNotFired(Move move)
        {
            //move Enemy on the same x/y as the obstacle
            _enemy.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Obstacle).X;
            _enemy.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Obstacle).Y;

            //bool for obstacleeventcheck
            bool ObstacleEventIsFired = false;

            //subscribe to obstacleevent
            _collisionDetector.ObstacleCollision += _collisionDetector_ObstacleCollision;

            //move the Enemy around
            _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move);

            Assert.False(ObstacleEventIsFired);

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
        public void EnemyMovesOnWall_enemyMoveEqualsStop(Move move)
        {
            //move Enemy on the same x/y as the wall
            _enemy.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Wall).X;
            _enemy.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Wall).Y;

            //simulate a move
            _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move);

            Assert.True(_enemy.CurrentMove == Move.Stop);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void EnemyMovesAgainstBorder_enemyMoveEqualsStop(Move move)
        {
            //move Enemy next to the wall in the upper right corner
            _enemy.X = _gameValues.PlayCanvasWidth;
            _enemy.Y = 0;

            //simulate a move
            _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move);

            Assert.True(_enemy.CurrentMove == Move.Stop);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void EnemyTouchesEndpoint_enemyMoveEqualsStop(Move move)
        {
            //move Enemy on the same x/y as the endpoint
            _enemy.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.EndPoint).X;
            _enemy.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.EndPoint).Y;

            //simulate a move
            _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move);

            Assert.True(_enemy.CurrentMove == Move.Stop);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void EnemyHitsAPlayer_EnemyHitEventIsFired(Move move)
        {
            //move Enemy on the same x/y as the player
            _enemy.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Player).X;
            _enemy.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.Player).Y;

            //bool for cointeventcheck
            bool EnemyEventIsFired = false;

            //subscribe to coinevent
            _collisionDetector.EnemyCollision += (sender, args) => { EnemyEventIsFired = true; };

            //move the Enemy around
            _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move);

            Assert.True(EnemyEventIsFired);
        }

        [TestCase(Move.Down)]
        [TestCase(Move.Up)]
        [TestCase(Move.Left)]
        [TestCase(Move.Right)]
        [TestCase(Move.Stop)]
        public void EnemyHitsSpawnPoint_CollisionIsClear(Move move)
        {
            //move enemy on the same x/y as the spawnpoint
            _enemy.X = _playgroundObjects.Find(x => x.ObjectType == ObjectType.SpawnPoint).X;
            _enemy.Y = _playgroundObjects.Find(x => x.ObjectType == ObjectType.SpawnPoint).Y;

            
            Assert.True(Collision.Clear == _collisionDetector.ObjectCollision(_playgroundObjects, _enemy, move));
        }
    }
}