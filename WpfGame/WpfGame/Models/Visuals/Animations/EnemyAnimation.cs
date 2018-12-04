using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WpfGame.Controllers;
using WpfGame.Generals;

namespace WpfGame.Models.Animations
{
    public class EnemyAnimation
    {
        private Dictionary<EnemyFacing, BitmapImage> _enemyFacingBitmapImages;
        private EnemyFacing currentEnemyFacing;
        private Move _lastMove;     

        public EnemyAnimation()
        {
            _enemyFacingBitmapImages = new Dictionary<EnemyFacing, BitmapImage>();
            _lastMove = Move.Right;
        }

        public void LoadPacmanImages()
        {
            _enemyFacingBitmapImages.Add(EnemyFacing.Up, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Enemy/blinky-up.png")));
            _enemyFacingBitmapImages.Add(EnemyFacing.Down, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Enemy/blinky-down.png")));
            _enemyFacingBitmapImages.Add(EnemyFacing.Left, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Enemy/blinky-left.png")));
            _enemyFacingBitmapImages.Add(EnemyFacing.Right, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Enemy/blinky-right.png")));
        }

        public BitmapImage SetAnimation(Move move)
        {
            if (move == Move.Stop)
            {
                move = _lastMove;
            }

            switch (move)
            {
                case Move.Down:
                    currentEnemyFacing = EnemyFacing.Down;
                    _lastMove = move;
                    break;
                case Move.Up:
                    currentEnemyFacing = EnemyFacing.Up;
                    _lastMove = move;
                    break;
                case Move.Left:
                    currentEnemyFacing = EnemyFacing.Left;
                    _lastMove = move;
                    break;
                case Move.Right:
                    currentEnemyFacing = EnemyFacing.Right;
                    _lastMove = move;
                    break;
            }
            return _enemyFacingBitmapImages[currentEnemyFacing];
        }
    }
}
