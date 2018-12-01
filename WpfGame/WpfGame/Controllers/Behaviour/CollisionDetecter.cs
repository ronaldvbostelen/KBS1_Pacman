using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfGame.Controllers.Creatures;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Behaviour
{
    public class CollisionDetecter
    {
        private GameValues _gameValues;
        public event EventHandler<ImmovableEventArgs> CoinCollision;
        public event EventHandler EnemyCollision;
        public event EventHandler ObstacleCollision;
        public event EventHandler EndpointCollision;

        public CollisionDetecter(GameValues gameValues)
        {
            _gameValues = gameValues;
        }

        private bool LeftBorderOfPlaygroundCollision(double xOfObject, double nextMove) => xOfObject - nextMove <= 0;

        private bool RightBorderOfPlaygroundCollision(double playgroundWidth, double objectWidth, double xOfObject,
            double nextMove) => xOfObject + nextMove + objectWidth >= playgroundWidth;

        private bool TopBorderOfPlaygroundCollision(double yOfObject, double nextMove) => yOfObject - nextMove <= 0;

        private bool BottomBorderOfPlaygroundCollision(double playgroundHeight, double objectHeight,
            double yOfObject, double nextMove) => yOfObject + objectHeight + nextMove >= playgroundHeight;

        //we check if the sprite doesnt walk off the screen
        private bool BorderCollision(MovableObject movable, Move move)
        {
            switch (move)
            {
                case Move.Up:
                    return TopBorderOfPlaygroundCollision(movable.Y, _gameValues.Movement);
                case Move.Down:
                    return BottomBorderOfPlaygroundCollision(_gameValues.PlayCanvasHeight, movable.Image.Height,
                        movable.Y,
                        _gameValues.Movement);
                case Move.Left:
                    return LeftBorderOfPlaygroundCollision(movable.X, _gameValues.Movement);
                case Move.Right:
                    return RightBorderOfPlaygroundCollision(_gameValues.PlayCanvasWidth, movable.Image.Width, movable.X,
                        _gameValues.Movement);
                default:
                    return false;
            }
        }

        public Collision ObjectCollision(List<IPlaygroundObject> objectList, MovableObject movable, Move move)
        {
            double addToX = 0;
            double addToY = 0;

            switch (move)
            {
                case Move.Down:
                    addToY = _gameValues.Movement;
                    break;
                case Move.Up:
                    addToY = -_gameValues.Movement;
                    break;
                case Move.Left:
                    addToX = -_gameValues.Movement;
                    break;
                case Move.Right:
                    addToX = _gameValues.Movement;
                    break;
            }

            Rect moveObject = new Rect(new Point(movable.X + addToX, movable.Y + addToY),
                new System.Windows.Size(movable.Image.Width, movable.Image.Height));

            foreach (var obj in objectList)
            {
                Rect tileRect = new Rect(new Point(obj.X, obj.Y),
                    new System.Windows.Size(obj.Image.Width, obj.Image.Height));

                //the collisiondector will fire an event when a gamebreaking collision took place, it will also return a collisionvalue to the caller.
                if (moveObject.IntersectsWith(tileRect))
                {
                    switch (obj.ObjectType)
                    {
                        case ObjectType.Player:
                            if (movable.ObjectType == ObjectType.Enemy)
                            {
                                OnEnemyCollision();
                            }
                            return Collision.Player;
                        case ObjectType.Enemy:
                            if (movable.ObjectType == ObjectType.Player)
                            {
                                OnEnemyCollision();
                            }
                            return Collision.Enemy;
                        case ObjectType.EndPoint:
                            //we have to compute the amount of intersection, so only when our player is for 99% on the endtile the game will end
                            var intersectedRec = Rect.Intersect(moveObject, tileRect);
                            if ((intersectedRec.Width * intersectedRec.Height) * 100f / (moveObject.Width * moveObject.Height) > 99)
                            {
                                OnEndpointCollision();
                            }
                            break;
                        case ObjectType.SpawnPoint:
                        case ObjectType.Wall:
                            return Collision.Wall;
                        case ObjectType.Coin:
                            // we invoke the coin event when the movableobject hits an active obstacle
                            var coin = (ImmovableObject) obj;
                            if (coin.State)
                            {
                                OnCoinCollision(new ImmovableEventArgs(coin));
                            }
                            break;
                        case ObjectType.Obstacle:
                            var obstacle = (ImmovableObject)obj;
                            // we invoke the obstacle event when the movableobject hits an active obstacle
                            if (obstacle.State)
                            {
                                OnObstacleCollision();
                            }
                            break;
                    }
                }
            }
            return BorderCollision(movable,move) ? Collision.Border : Collision.Clear;
        }

        protected virtual void OnCoinCollision(ImmovableEventArgs args)
        {
            CoinCollision?.Invoke(this, args);
        }   

        protected virtual void OnEnemyCollision()
        {
            EnemyCollision?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnObstacleCollision()
        {
            ObstacleCollision?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnEndpointCollision()
        {
            EndpointCollision?.Invoke(this, EventArgs.Empty);
        }
    }
}