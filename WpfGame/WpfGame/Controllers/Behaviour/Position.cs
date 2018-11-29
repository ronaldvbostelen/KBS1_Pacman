using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfGame.Controllers.Creatures;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Behaviour
{
    public class Position
    {
        public int Left { get; set; }
        public int Top { get; set; }
        private GameValues _gameValues;

        public Position(GameValues gameValues)
        {
            _gameValues = gameValues;
        }

        public void UpdatePosition(MovableObject sprite)
        {
            switch (sprite.CurrentMove)
            {
                case Move.Stop:
                    break;
                case Move.Up:
                    sprite.Y -= _gameValues.Movement;
                    break;
                case Move.Down:
                    sprite.Y += _gameValues.Movement;
                    break;
                case Move.Left:
                    sprite.X -= _gameValues.Movement;
                    break;
                case Move.Right:
                    sprite.X += _gameValues.Movement;
                    break;
            }
        }
    }
}
