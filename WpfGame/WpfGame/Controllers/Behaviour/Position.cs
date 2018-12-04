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
        public List<IPlaygroundObject> PlaygroundObjects { private get; set; }
        private static Random _random;
        private GameValues _gameValues;
        private List<Move> _enemyMoves;
        private Move _lastMove;
        private int _stepsInDirection; // necessary for smooth movement of enemy

        public Position(GameValues gameValues)
        {
            _gameValues = gameValues;
            CollisionDetecter = new CollisionDetecter(_gameValues);
            _random = new Random();
            _enemyMoves = new List<Move>
            {
                Move.Left,
                Move.Right,
                Move.Down,
                Move.Up
            };
            _lastMove = new Move();
            _stepsInDirection = 0;
        }


        //this function is called in the game-engine. We first check if the nextmove is possible (the move from userinput) if its possible we set that move, if its impossible
        //a gamebreaking event has been fired by the CollisionDector or we try the CurrentMove and based on that outcome we move/stop/break te game.
        public void ProcessMove(MovableObject sprite)
        {
            if (CollisionDetecter.ObjectCollision(PlaygroundObjects, sprite, sprite.NextMove) == Collision.Clear)
            {
                sprite.CurrentMove = sprite.NextMove;
            }
            else if (CollisionDetecter.ObjectCollision(PlaygroundObjects, sprite, sprite.CurrentMove) !=
                     Collision.Clear)
            {
                sprite.CurrentMove = sprite.NextMove = Move.Stop;
            }

            UpdatePosition(sprite);
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

        public void EnemyProcessMove(MovableObject enemy)
        {
            List<Move> current = DeterminePossibleMoves(enemy);
            List<Move> newOptions = CompareList(_enemyMoves, current);
            if (enemy.CurrentMove == Move.Up || enemy.CurrentMove == Move.Down)
            {
                if (newOptions.Exists(x => x == Move.Left) && (newOptions.Exists(x => x == Move.Right)))
                {
                    if (_random.NextDouble() > 0.5)
                    {
                        enemy.NextMove = Move.Left;
                    }
                    else
                    {
                        enemy.NextMove = Move.Right;
                    }
                }
                else if (newOptions.Exists(x => x == Move.Left) || (newOptions.Exists(x => x == Move.Right)))
                {
                    if (newOptions.Exists(x => x == Move.Left))
                    {
                        enemy.NextMove = Move.Left;
                    }
                    else
                    {
                        enemy.NextMove = Move.Right;
                    }
                }
            }
            else if ((enemy.CurrentMove == Move.Left) || (enemy.CurrentMove == Move.Right))
            {
                if (newOptions.Exists(x => x == Move.Up) && (newOptions.Exists(x => x == Move.Down)))
                {
                    if (_random.NextDouble() > 0.5)
                    {
                        enemy.NextMove = Move.Up;
                    }
                    else
                    {
                        enemy.NextMove = Move.Down;
                    }
                }
                else if (newOptions.Exists(x => x == Move.Up) || (newOptions.Exists(x => x == Move.Down)))
                {
                    if (newOptions.Exists(x => x == Move.Up))
                    {
                        enemy.NextMove = Move.Down;
                    }
                    else
                    {
                        enemy.NextMove = Move.Down;
                    }
                }
            }

            else if (enemy.CurrentMove == Move.Stop && enemy.CurrentMove == Move.Stop)
            {
                if (_stepsInDirection == 1)
                {
                    enemy.NextMove = GetOppositeMove(_lastMove);
                    _stepsInDirection = 0;
                }
                else
                {
                    Move dealer = new Move();
                    if (current.Count == 1)
                    {
                        dealer = current[_random.Next(0, current.Count)];
                    }
                    else
                    {
                        current.Remove(GetOppositeMove(_lastMove));
                        dealer = current[_random.Next(0, current.Count)];
                    }

                    _stepsInDirection = 0;
                    enemy.NextMove = dealer;
                }

                RememberMove(enemy, enemy.NextMove);
            }
            else
            {
                _stepsInDirection++;
            }

            current = newOptions;
        }

        // in order to list the 'new' possible movement directions
        private List<Move> CompareList(List<Move> lastPossibleMoves, List<Move> currentPossibleMoves)
        {
            List<Move> nowPossible = new List<Move>();
            if (currentPossibleMoves.Except(lastPossibleMoves).ToList()?.Any() != true)
            {
                nowPossible = currentPossibleMoves.Except(lastPossibleMoves).ToList();
            }

            return nowPossible;
        }

        //determines all possible directions
        private List<Move> DeterminePossibleMoves(MovableObject enemy)
        {
            List<Move> possibleMoves = new List<Move>();
            if (PossibleMove(enemy, Move.Up))
            {
                possibleMoves.Add(Move.Up);
            }

            if (PossibleMove(enemy, Move.Down))
            {
                possibleMoves.Add(Move.Down);
            }

            if (PossibleMove(enemy, Move.Left))
            {
                possibleMoves.Add(Move.Left);
            }

            if (PossibleMove(enemy, Move.Right))
            {
                possibleMoves.Add(Move.Right);
            }

            return possibleMoves;
        }

        //determines if direction is possible
        private bool PossibleMove(MovableObject enemy, Move move) =>
            CollisionDetecter.ObjectCollision(PlaygroundObjects, enemy, move) == Collision.Clear;

        //to prevent 'ping-pong effect': for example: if lastmove before collision is Left, enemy should move Up or Down
        private void RememberMove(MovableObject enemy, Move move)
        {
            if (move != Move.Stop && move != enemy.CurrentMove)
                _lastMove = move;
        }

        // navigate the enemy to opposite direction
        private Move GetOppositeMove(Move move)
        {
            switch (move)
            {
                case Move.Down:
                    return Move.Up;
                case Move.Up:
                    return Move.Down;
                case Move.Left:
                    return Move.Right;
                case Move.Right:
                    return Move.Left;
                default:
                    return Move.Stop;
            }
        }
    }
}