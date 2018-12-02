using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace WpfGame.Models.Animations
{
    public class ObstacleAnimation
    {
        private Dictionary<bool, BitmapImage> _obstacleDict;

        public ObstacleAnimation()
        {
            _obstacleDict = new Dictionary<bool, BitmapImage>();
        }

        public void LoadObstacleImages()
        {
            _obstacleDict.Add(true, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/obstacle-on.png")));
            _obstacleDict.Add(false, new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/obstacle-off.png")));
        }

        public BitmapImage SetObstacleImage(bool b) => _obstacleDict[b];
    }
}