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

        //this function is called in the game-engine. We first check if the nextmove is possible (the move from userinput) if its possible we set that move, if its impossible
        //a gamebreaking event has been fired by the CollisionDector or we try the CurrentMove and based on that outcome we move/stop/break te game.
        public void ProcessMove(MovableObject sprite)
        {
            if (CollisionDetecter.ObjectCollision(PlaygroundObjects, sprite, sprite.NextMove, sprite.ObjectType == ObjectType.Enemy) == Collision.Clear)
            {
                sprite.CurrentMove = sprite.NextMove;
            }
            else if (CollisionDetecter.ObjectCollision(PlaygroundObjects, sprite, sprite.CurrentMove, sprite.ObjectType == ObjectType.Enemy) != Collision.Clear)
            {
                sprite.CurrentMove = sprite.NextMove = Move.Stop;
            }

            UpdatePosition(sprite);
        }
    }
}
