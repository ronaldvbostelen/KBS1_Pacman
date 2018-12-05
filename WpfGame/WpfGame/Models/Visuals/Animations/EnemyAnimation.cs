using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WpfGame.Generals;

namespace WpfGame.Models.Visuals.Animations
{
    public class EnemyAnimation
    {
        private Dictionary<EnemyFacing, BitmapImage> _enemyFacingBitmapImages;
        private EnemyFacing _currentEnemyFacing;
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
                    _currentEnemyFacing = EnemyFacing.Down;
                    _lastMove = move;
                    break;
                case Move.Up:
                    _currentEnemyFacing = EnemyFacing.Up;
                    _lastMove = move;
                    break;
                case Move.Left:
                    _currentEnemyFacing = EnemyFacing.Left;
                    _lastMove = move;
                    break;
                case Move.Right:
                    _currentEnemyFacing = EnemyFacing.Right;
                    _lastMove = move;
                    break;
            }
            return _enemyFacingBitmapImages[_currentEnemyFacing];
        }
    }
}
