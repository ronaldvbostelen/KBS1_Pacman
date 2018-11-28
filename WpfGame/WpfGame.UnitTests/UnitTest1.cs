using System;
using System.Net.Mime;
using NUnit.Framework;
using WpfGame;
using WpfGame.Controllers;
using WpfGame.Controllers.Views;
using WpfGame.Values;
using WpfGame.Views;
using WpfGame.Controllers.Behaviour;
using WpfGame.Generals;
using WpfGame.Models;

namespace WpfGame.UnitTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            Assert.Throws<Exception>(() => new GameViewController(new MainWindow(), "dummy"));
        }

        [Test]
        public void TestUpdatePosition()
        {

            var _gameValues = new GameValues();
            Position pos = new Position(_gameValues);

            var _gameView = new GameView();
            _gameValues.PlayCanvasHeight = 600;
            _gameValues.PlayCanvasWidth = 784;
            _gameValues.UpDownMovement = _gameValues.PlayCanvasHeight / 200;
            _gameValues.LeftRightMovement = _gameValues.PlayCanvasWidth / 200;
            MovableObject _sprite = new MovableObject(ObjectType.Player,null, 50,50,10,10);
            var currpos = _sprite.X;
            _sprite.CurrentMove = Move.Right;
            pos.UpdatePosition(_sprite);
            Assert.AreEqual(currpos + _gameValues.LeftRightMovement,_sprite.X);
        }
    }
}
