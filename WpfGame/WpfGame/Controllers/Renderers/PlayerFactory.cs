using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Models.Playgroundobjects;
using WpfGame.Values;

namespace WpfGame.Controllers.Renderers
{
    public class PlayerFactory 
    {
        private GameValues _gameValues;
        private BitmapImage _playersFirstFace;
        private double _playerWidth, _playerHeight, _playerStartPointCorrection;


        public void LoadFactory(GameValues gameValues)
        {
            _gameValues = gameValues;

            _playersFirstFace = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"));
            _playerWidth = _gameValues.TileWidth * 0.93;
            _playerHeight = _gameValues.TileHeight * 0.93;
            _playerStartPointCorrection = 1.003;
        }

        public MovableObject LoadPlayer(List<IPlaygroundObject> list)
        {
            try
            {
                var startpoint = list.First(x => x.ObjectType == ObjectType.StartPoint);
                return new MovableObject(ObjectType.Player, new Image { Source = _playersFirstFace }, _playerWidth, _playerHeight, startpoint.X * _playerStartPointCorrection, startpoint.Y * _playerStartPointCorrection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentNullException("woeeeps", "your playboards doesn't contain a startpoint");
            }   
        }

        public MovableObject DrawPlayer(MovableObject player, Canvas canvas)
        {
            Canvas.SetTop(player.Image, player.Y);
            Canvas.SetLeft(player.Image, player.X);
            canvas.Children.Add(player.Image);
            return player;
        }
    }
}