using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfGame.Controllers.Creatures;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Behaviour
{
    public class Position
    {
        public CollisionDetecter CollisionDetecter { get; }
        public List<IPlaygroundObject> PlaygroundObjects { get; set; }
        private GameValues _gameValues;

        public Position(GameValues gameValues)
        {
            _gameValues = gameValues;
            CollisionDetecter = new CollisionDetecter(_gameValues);
        }

        private void UpdatePosition(MovableObject sprite)
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

        public void ProcessMove(MovableObject sprite, Move move)
        {
            if (CollisionDetecter.ObjectCollision(PlaygroundObjects, sprite, move) == Collision.Clear)
            {
                sprite.CurrentMove = move;
            }
            else if (CollisionDetecter.ObjectCollision(PlaygroundObjects, sprite, sprite.CurrentMove) != Collision.Clear)
            {
                sprite.CurrentMove = sprite.NextMove = Move.Stop;
            }

            UpdatePosition(sprite);
        }
    }
}
