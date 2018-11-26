using System;
using System.Collections.Generic;
using System.Windows;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Behaviour
{
    public class HitTester
    {
        private GameValues _gameValues;

        public HitTester(GameValues gameValues)
        {
            _gameValues = gameValues;
        }

        public bool HitLeftBorderOfPlayground(double xOfObject, double nextMove) => xOfObject - nextMove >= 0;

        public bool HitRightBorderOfPlayground(double playgroundWidth, double objectWidth, double xOfObject,
            double nextMove) => xOfObject + nextMove + objectWidth <= playgroundWidth;

        public bool HitTopBorderOfPlayground(double yOfObject, double nextMove) => yOfObject - nextMove >= 0;

        public bool HitBottomBorderOfPlayground(double playgroundHeight, double objectHeight,
            double yOfObject, double nextMove) => yOfObject + objectHeight + nextMove <= playgroundHeight;

        public bool HitMeBabyHitMeBabyOneMoreTime<T>(List<T> objectList, Player pacman, NextMove nextMove, Predicate<T> predicate) where T : PlaygroundObject
        {
            double addToX = 0;
            double addToY = 0;

            switch (nextMove)
            {
                case NextMove.Down:
                    addToY = _gameValues.UpDownMovement;
                    break;
                case NextMove.Up:
                    addToY = -_gameValues.UpDownMovement;
                    break;
                case NextMove.Left:
                    addToX = -_gameValues.LeftRightMovement;
                    break;
                case NextMove.Right:
                    addToX = _gameValues.LeftRightMovement;
                    break;
            }

            Rect pacmanRect = new Rect(new Point(pacman.X + addToX, pacman.Y + addToY),
                new System.Windows.Size(pacman.PlayerImage.Width, pacman.PlayerImage.Height));

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