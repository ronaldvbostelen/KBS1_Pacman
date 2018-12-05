using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using NUnit.Framework;
using WpfGame;
using WpfGame.Controllers.Behaviour;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Models.Playgroundobjects;
using WpfGame.Values;

namespace WPF_Game.Unittests
{
    [TestFixture(Description = "Score testclases")]
    public class ScoreTests
    {
        private readonly GameValues _gameValues;
        private readonly CollisionDetecter _collisionDetecter;
        private readonly List<IPlaygroundObject> _playgroundObjects;
        private readonly MovableObject _player;
        private readonly MovableObject _enemy;
        
        public ScoreTests()
        {
            if (Application.ResourceAssembly == null)
            {
                Application.ResourceAssembly = typeof(MainWindow).Assembly;
            }

            _gameValues = new GameValues {PlayCanvasHeight = 600, PlayCanvasWidth = 784};
            _gameValues.HeigthWidthRatio = _gameValues.PlayCanvasHeight / _gameValues.PlayCanvasWidth;
            _gameValues.AmountOfXtiles = 20;
            _gameValues.AmountofYtiles = Math.Round(_gameValues.AmountOfXtiles * _gameValues.HeigthWidthRatio);
            _gameValues.TileWidth = _gameValues.PlayCanvasWidth / _gameValues.AmountOfXtiles;
            _gameValues.TileHeight = _gameValues.PlayCanvasHeight / _gameValues.AmountofYtiles;
            _gameValues.Movement = 2.5;
            
            _collisionDetecter = new CollisionDetecter(_gameValues);

            _playgroundObjects = new List<IPlaygroundObject>
            {
                new ImmovableObject(ObjectType.Coin, new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                }, 100, 100, 50, 5, true)
            };

            _player = new MovableObject(ObjectType.Player,
                new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                }, 50, 50, 75, 25);

            _enemy = new MovableObject(ObjectType.Enemy,
                new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                }, 50, 50, 75, 25);

        }

        [Test]
        public void IncrementScore_AfterPlayerHitsACoin()
        {
            var startscore = 0;

            //subscribe to coinevent
            _collisionDetecter.CoinCollision += CollisionDetecterCoinCollision;

            //simulate player hitting a coint
            _player.NextMove = Move.Right;
            _collisionDetecter.ObjectCollision(_playgroundObjects, _player, _player.NextMove);

            Assert.True(startscore == 1);

            void CollisionDetecterCoinCollision(object sender, ImmovableEventArgs e)
            {
                startscore++;
            }
        }


        [Test]
        public void IgnoreCoin_AfterEnemyHitsACoin()
        {
            var startscore = 0;

            //subscribe to coinevent
            _collisionDetecter.CoinCollision += CollisionDetecterCoinCollision;

            //simulate player hitting a coint
            _enemy.NextMove = Move.Right;
            _collisionDetecter.ObjectCollision(_playgroundObjects, _enemy, _enemy.NextMove);

            Assert.False(startscore == 1);

            void CollisionDetecterCoinCollision(object sender, ImmovableEventArgs e)
            {
                startscore++;
            }
        }
    }
}

