using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfGame.Controllers;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Generals;
using WpfGame.Values;

namespace UnitTests
{
    [TestClass]
    public class MovePlayerTest
    {
        private readonly Position _position;
        private readonly GameValues _gameValues;
        private readonly Sprite _sprite;

        public MovePlayerTest()
        {
            _gameValues = new GameValues();
            _position = new Position(_gameValues);
            _sprite = new Player(20, 20, 0, 0);
        }

        [TestMethod]
        public void UpdatePosition_MoveStop_ReturnsNull()
        {
            var result = _position.UpdatePosition(_sprite, Move.Stop);
            Assert.AreEqual(null, result);
        }
    }
}
