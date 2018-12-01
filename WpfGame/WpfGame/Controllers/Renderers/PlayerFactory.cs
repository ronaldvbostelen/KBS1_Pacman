using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Renderer
{
    public class PlayerFactory 
    {
        private GameValues _gameValues;
        private BitmapImage _playersFirstFace;
        private double playerWidth, playerHeight, playerStartPointCorrection;


        public void LoadFactory(GameValues gameValues)
        {
            _gameValues = gameValues;

            _playersFirstFace = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"));
            playerWidth = _gameValues.TileWidth * 0.93;
            playerHeight = _gameValues.TileHeight * 0.93;
            playerStartPointCorrection = 1.003;
        }

        public MovableObject LoadPlayer(List<IPlaygroundObject> list)
        {
            try
            {
                var startpoint = list.First(x => x.ObjectType == ObjectType.StartPoint);
                return new MovableObject(ObjectType.Player, new Image { Source = _playersFirstFace }, playerWidth, playerHeight, startpoint.X * playerStartPointCorrection, startpoint.Y * playerStartPointCorrection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentNullException("woeeeps", "your playboards doesn't contain a startpoint");
            }   
        }

        public void DrawPlayer(MovableObject player, Canvas canvas)
        {
            Canvas.SetTop(player.Image, player.Y);
            Canvas.SetLeft(player.Image, player.X);
            canvas.Children.Add(player.Image);
        }
    }
}