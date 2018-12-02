using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WpfGame;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Views;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;
using WpfGame.Views;
using Assert = NUnit.Framework.Assert;

namespace WpfGame.UnitTests
{
    [TestFixture(Description = "WPF_PACMAN_UNIT_TESTS")]
    public class MovePlayerTest
    {
        private readonly Position _position;
        private readonly GameValues _gameValues;
        private readonly MovableObject _player;
        
        public MovePlayerTest()
        {
            //this piece of code gave me headache. you'll need it in every testclass. // NEVER REMOVE IT //
            if (Application.ResourceAssembly == null)
            {
                Application.ResourceAssembly = typeof(MainWindow).Assembly;
            }
            
            _gameValues = new GameValues();
            _gameValues.PlayCanvasHeight = 600;
            _gameValues.PlayCanvasWidth = 784;
            _gameValues.Movement = 2.5;
            _gameValues.Movement = 2.5;
            _position = new Position(_gameValues);
            _position.PlaygroundObjects = new List<IPlaygroundObject>{new StaticObject(ObjectType.Player,
                new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                }, 50, 50, 100, 100)};

            _player = new MovableObject(ObjectType.Player,
                new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                }, 50, 50, 10, 10);
        }
        
        [Test]
        public void Player_isNotNull()
        {
            Assert.IsNotNull(_player);
        }

        [Test]
        public void UpdatePosition_MoveStop_ReturnsEqualXY()
        {
            var x = _player.X;
            var y = _player.Y;
            _player.NextMove = Move.Stop;
             _position.ProcessMove(_player);
           Assert.AreEqual(x+y, _player.X + _player.Y);
         }

        [Test]
        public void UpdatePosition_MoveUp_ReturnsCorrectDouble()
        {
            double expected = _player.Y - _gameValues.Movement;
            _player.NextMove = Move.Up;
            _position.ProcessMove(_player);
            Assert.AreEqual(expected, _player.Y);
        }

        [Test]
        public void UpdatePosition_MoveDown_ReturnsCorrectDouble()
        {
            double expected = _player.Y + _gameValues.Movement;
            _player.NextMove = Move.Down;
            _position.ProcessMove(_player);
            Assert.AreEqual(expected, _player.Y);
        }

        [Test]
        public void UpdatePosition_MoveRight_ReturnsCorrectDouble()
        {
            double expected = _player.X + _gameValues.Movement;
            _player.NextMove = Move.Right;
            _position.ProcessMove(_player);
            Assert.AreEqual(expected, _player.X);
        }

        [Test]
        public void UpdatePosition_MoveLeft_ReturnsCorrectDouble()
        {
            double expected = _player.X - _gameValues.Movement;
            _player.NextMove = Move.Left;
            _position.ProcessMove(_player);
            Assert.AreEqual(expected, _player.X);
        }
    }
}


