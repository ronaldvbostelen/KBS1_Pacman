﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Models.Playgroundobjects;
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

            //simulate next move
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
                new Size(movable.Image.Width, movable.Image.Height));

            //this list contains all the playgroundobjects. we loop through it end every object (its position and demension) is passed to the Rect. we then use the .IntersectWith
            //function to check if the moveableobject (a player or a enemy) hits something on the playingfield.
            foreach (var obj in objectList)
            {
                Rect tileRect = new Rect(new Point(obj.X, obj.Y),
                    new Size(obj.Image.Width, obj.Image.Height));

                //the collisiondector will fire an event when a gamebreaking collision took place, it will also return a collisionvalue to the caller.
                if (moveObject.IntersectsWith(tileRect))
                {
                    switch (obj.ObjectType)
                    {
                        case ObjectType.Player:
                            // we invoke the enemy event when the enemy hits the player
                            if (movable.ObjectType == ObjectType.Enemy)
                            {
                                OnEnemyCollision();
                            }
                            break;
                        case ObjectType.Enemy:
                            // we invoke the enemy event when the player hits an enemy
                            if (movable.ObjectType == ObjectType.Player)
                            {
                                OnEnemyCollision();
                            }
                            break;
                        case ObjectType.EndPoint:
                            //only when the player hits the endpoint we invoke the event, an enemyhit will be ignored
                            if (movable.ObjectType == ObjectType.Player)
                            {
                                //we compute the amount of intersection, only when our player is for >99% on the endtile the game will end
                                var intersectedRec = Rect.Intersect(moveObject, tileRect);
                                if (intersectedRec.Width * intersectedRec.Height * (100 /
                                    (moveObject.Width * moveObject.Height)) > 99)
                                {
                                    OnEndpointCollision();
                                }
                            }
                            break;
                        case ObjectType.SpawnPoint:
                            //spawnpoint equals wall for player, an enemy can pass it
                            if (movable.ObjectType == ObjectType.Player)
                            {
                                return Collision.Wall;
                            }
                            break;
                        case ObjectType.Wall:
                            return Collision.Wall;
                        case ObjectType.Coin:
                            // we invoke the coin event when the player hits an active coin
                            if (movable.ObjectType == ObjectType.Player)
                            {
                                var coin = (ImmovableObject)obj;
                                if (coin.State)
                                {
                                    //we compute the amount of intersection, only when our player has eaten the coin for >25% it will register as hit
                                    var intersectedRec = Rect.Intersect(moveObject, tileRect);
                                    if (intersectedRec.Width * intersectedRec.Height * (100 /
                                        (moveObject.Width * moveObject.Height)) > 25)
                                    {

                                        OnCoinCollision(new ImmovableEventArgs(coin));
                                    }

                                }
                            }
                            break;
                        case ObjectType.Obstacle:
                            // we invoke the obstacle event when the player hits an active obstacle
                            if (movable.ObjectType == ObjectType.Player)
                            {
                                var obstacle = (ImmovableObject) obj;
                                
                                if (obstacle.State)
                                {
                                    //we compute the amount of intersection, only when our player has hit the active obstacle for >35% it will register as hit
                                    var intersectedRec = Rect.Intersect(moveObject, tileRect);
                                    if (intersectedRec.Width * intersectedRec.Height * (100 /
                                        (moveObject.Width * moveObject.Height)) > 35)
                                    {
                                        OnObstacleCollision();
                                    }
                                        
                                }
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