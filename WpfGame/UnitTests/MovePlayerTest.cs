using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfGame.Controllers;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Generals;
using WpfGame.Values;
using WpfGame.Views;

namespace UnitTests
{
    [TestClass]
    public class MovePlayerTest
    {
        private readonly Position _position;
        private readonly GameValues _gameValues;
        private readonly Sprite _sprite;
        private readonly GameView _gameView;

        public MovePlayerTest()
        {
            _gameValues = new GameValues();
            _gameView = new GameView();
            _gameValues.PlayCanvasHeight = 600;
            _gameValues.PlayCanvasWidth = 784;
            _gameValues.UpDownMovement = _gameValues.PlayCanvasHeight / 200;
            _gameValues.LeftRightMovement = _gameValues.PlayCanvasWidth / 200;
            _position = new Position(_gameValues);
            _sprite = new Player(20, 20, 0, 0);
        }

        [TestMethod]
        public void UpdatePosition_MoveStop_ReturnsNull()
        {
            var result = _position.UpdatePosition(_sprite, Move.Stop);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void UpdatePosition_MoveUp_ReturnsCorrectDouble()
        {
            double expected = -3;
            var result = _position.UpdatePosition(_sprite, Move.Up);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UpdatePosition_MoveDown_ReturnsCorrectDouble()
        {
            double expected = 3;
            var result = _position.UpdatePosition(_sprite, Move.Down);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UpdatePosition_MoveRight_ReturnsCorrectDouble()
        {
            double expected = 3.92;
            var result = _position.UpdatePosition(_sprite, Move.Right);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UpdatePosition_MoveLeft_ReturnsCorrectDouble()
        {
            double expected = -3.92;
            var result = _position.UpdatePosition(_sprite, Move.Left);
            Assert.AreEqual(expected, result);
        }
    }
}
