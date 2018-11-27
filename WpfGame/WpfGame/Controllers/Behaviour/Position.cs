using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfGame.Controllers.Creatures;
using WpfGame.Generals;
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

        public double? UpdatePosition(Sprite sprite, Move move)
        {
            switch (move)
            {
                case Move.Stop:
                    return null;
                case Move.Up:
                    return sprite.Y -= _gameValues.UpDownMovement;
                case Move.Down:
                    return sprite.Y += _gameValues.UpDownMovement;
                case Move.Left:
                    return sprite.X -= _gameValues.LeftRightMovement;
                case Move.Right:
                    return sprite.X += _gameValues.LeftRightMovement;
            }

            return null;
        }
    }
}
