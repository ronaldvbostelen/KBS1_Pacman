using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using WpfGame.Controllers;
using WpfGame.Generals;

namespace WpfGame.Models.Animations
{
    public class PacmanAnimation
    {
        private Dictionary<PacmanFacing, BitmapImage> _pacmanFacingBitmapImages;
        private PacmanFacing currentPacmanFacing;
        private Move _lastMove;

        public PacmanAnimation()
        {
            _pacmanFacingBitmapImages = new Dictionary<PacmanFacing, BitmapImage>();
            _lastMove = Move.Right;
        }

        public void LoadPacmanImages()
        {
            _pacmanFacingBitmapImages.Add(PacmanFacing.Up, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-up-halfopenjaw.png")));
            _pacmanFacingBitmapImages.Add(PacmanFacing.UpOpen, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-up-openjaw.png")));
            _pacmanFacingBitmapImages.Add(PacmanFacing.Down, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-down-halfopenjaw.png")));
            _pacmanFacingBitmapImages.Add(PacmanFacing.DownOpen, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-down-openjaw.png")));
            _pacmanFacingBitmapImages.Add(PacmanFacing.Left, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-left-halfopenjaw.png")));
            _pacmanFacingBitmapImages.Add(PacmanFacing.LeftOpen, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-left-openjaw.png")));
            _pacmanFacingBitmapImages.Add(PacmanFacing.Right, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png")));
            _pacmanFacingBitmapImages.Add(PacmanFacing.RightOpen, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-openjaw.png")));
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
                    currentPacmanFacing = currentPacmanFacing == PacmanFacing.Down ? PacmanFacing.DownOpen : PacmanFacing.Down;
                    _lastMove = move;
                    break;
                case Move.Up:
                    currentPacmanFacing = currentPacmanFacing == PacmanFacing.UpOpen ? PacmanFacing.Up : PacmanFacing.UpOpen;
                    _lastMove = move;
                    break;
                case Move.Left:
                    currentPacmanFacing = currentPacmanFacing == PacmanFacing.Left ? PacmanFacing.LeftOpen : PacmanFacing.Left;
                    _lastMove = move;
                    break;
                case Move.Right:
                    currentPacmanFacing = currentPacmanFacing == PacmanFacing.Right ? PacmanFacing.RightOpen : PacmanFacing.Right;
                    _lastMove = move;
                    break;
            }

            return _pacmanFacingBitmapImages[currentPacmanFacing];
        }
    }
}