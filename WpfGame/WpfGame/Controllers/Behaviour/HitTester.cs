﻿using System;
using System.Collections.Generic;
using System.Windows;
using WpfGame.Controllers.Creatures;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Behaviour
{
    public class CollisionDetecter
    {
        private GameValues _gameValues;

        public CollisionDetecter(GameValues gameValues)
        {
            _gameValues = gameValues;
        }

        public bool LeftBorderOfPlaygroundCollision(double xOfObject, double nextMove) => xOfObject - nextMove <= 0;

        public bool RightBorderOfPlaygroundCollision(double playgroundWidth, double objectWidth, double xOfObject,
            double nextMove) => xOfObject + nextMove + objectWidth >= playgroundWidth;

        public bool TopBorderOfPlaygroundCollision(double yOfObject, double nextMove) => yOfObject - nextMove <= 0;

        public bool BottomBorderOfPlaygroundCollision(double playgroundHeight, double objectHeight,
            double yOfObject, double nextMove) => yOfObject + objectHeight + nextMove >= playgroundHeight;

        public bool BorderCollision(Sprite sprite, Move move)
        {   
            switch (move)
            {
                case Move.Up:
                    return TopBorderOfPlaygroundCollision(sprite.Y, _gameValues.UpDownMovement);
                case Move.Down:
                    return BottomBorderOfPlaygroundCollision(_gameValues.PlayCanvasHeight, sprite.Image.Height, sprite.Y,
                        _gameValues.UpDownMovement);
                case Move.Left:
                    return LeftBorderOfPlaygroundCollision(sprite.X, _gameValues.LeftRightMovement);
                case Move.Right:
                    return RightBorderOfPlaygroundCollision(_gameValues.PlayCanvasWidth, sprite.Image.Width, sprite.X,
                        _gameValues.LeftRightMovement);
                default:
                    return false;
            }
        }
        
        public bool ObjectCollision<T>(List<T> objectList, Player pacman, Move move, Predicate<T> predicate) where T : PlaygroundObject
        {
            double addToX = 0;
            double addToY = 0;

            switch (move)
            {
                case Move.Down:
                    addToY = _gameValues.UpDownMovement;
                    break;
                case Move.Up:
                    addToY = -_gameValues.UpDownMovement;
                    break;
                case Move.Left:
                    addToX = -_gameValues.LeftRightMovement;
                    break;
                case Move.Right:
                    addToX = _gameValues.LeftRightMovement;
                    break;
            }

            Rect pacmanRect = new Rect(new Point(pacman.X + addToX, pacman.Y + addToY),
                new System.Windows.Size(pacman.Image.Width, pacman.Image.Height));

            foreach (var obj in objectList)
            {
                Rect tileRect = new Rect(new Point(obj.X, obj.Y), new System.Windows.Size(obj.Rectangle.Width, obj.Rectangle.Width));

                if (pacmanRect.IntersectsWith(tileRect) && predicate(obj))
                {
                    return true;
                }
            }

            return false;
        }
    }
}