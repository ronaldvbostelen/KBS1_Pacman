using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;
using System.Windows.Controls;

namespace WpfGame.Controllers.Renderer
{
    public class PlaygroundFactory 
    {
        private GameValues _gameValues;
        private Dictionary<ObjectType, BitmapImage> _imageDictionary;
        private Dictionary<ObjectType, Size> _demensionDictionary;
        private double coinCorrectionWidthPlacement, coinCorrectionHeightPlacement, obstacleCorrectionHeightPlacement;


        public PlaygroundFactory()
        {
            _imageDictionary = new Dictionary<ObjectType, BitmapImage>();
            _demensionDictionary = new Dictionary<ObjectType, Size>();
        }

        public void LoadFactory(GameValues gameValues)
        {
            _gameValues = gameValues;

            _imageDictionary.Add(ObjectType.Coin,
                new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/coin.png")));
            _imageDictionary.Add(ObjectType.EndPoint,
                new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/end.png")));
            _imageDictionary.Add(ObjectType.Obstacle,
                new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/obstacle-off.png")));
            _imageDictionary.Add(ObjectType.Path,
                new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/floor.png")));
            _imageDictionary.Add(ObjectType.SpawnPoint,
                new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/spawn.png")));
            _imageDictionary.Add(ObjectType.Wall,
                new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/wall.png")));
            _imageDictionary.Add(ObjectType.StartPoint,
                new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Objects/floor.png")));

            _demensionDictionary.Add(ObjectType.Coin,
                new Size(_gameValues.TileWidth * 0.65, _gameValues.TileHeight * 0.65));
            _demensionDictionary.Add(ObjectType.EndPoint, new Size(_gameValues.TileWidth, _gameValues.TileHeight));
            _demensionDictionary.Add(ObjectType.Obstacle, new Size(_gameValues.TileWidth, _gameValues.TileHeight));
            _demensionDictionary.Add(ObjectType.Path, new Size(_gameValues.TileWidth, _gameValues.TileHeight));
            _demensionDictionary.Add(ObjectType.SpawnPoint, new Size(_gameValues.TileWidth, _gameValues.TileHeight));
            _demensionDictionary.Add(ObjectType.Wall, new Size(_gameValues.TileWidth, _gameValues.TileHeight));
            _demensionDictionary.Add(ObjectType.StartPoint, new Size(_gameValues.TileWidth, _gameValues.TileHeight));

            coinCorrectionHeightPlacement = _gameValues.TileHeight * 0.17;
            coinCorrectionWidthPlacement = _gameValues.TileWidth * 0.17;
            obstacleCorrectionHeightPlacement = _gameValues.TileHeight * 0;
        }

        public List<IPlaygroundObject> LoadPlayground(List<TileMockup> mockups)
        {
            List<IPlaygroundObject> playground = new List<IPlaygroundObject>();
            ObjectType type;
            bool isCoin = false;
            bool isObstacle = false;
            int counter = 0;

            try
            {
                for (int i = 0; i < _gameValues.AmountofYtiles; i++)
                {
                    for (int j = 0; j < _gameValues.AmountOfXtiles; j++)
                    {
                        if (mockups[counter].IsSpawn)
                        {
                            type = ObjectType.SpawnPoint;
                        }
                        else if (mockups[counter].HasCoin)
                        {
                            type = ObjectType.Path;
                            isCoin = true;
                        }
                        else if (mockups[counter].HasObstacle)
                        {
                            type = ObjectType.Path;
                            isObstacle = true;
                        }
                        else if (mockups[counter].IsEnd)
                        {
                            type = ObjectType.EndPoint;
                        }
                        else if (mockups[counter].IsStart)
                        {
                            type = ObjectType.StartPoint;
                        }
                        else if (mockups[counter].IsWall)
                        {
                            type = ObjectType.Wall;
                        }
                        else
                        {
                            type = ObjectType.Path;
                        }

                        playground.Add(new StaticObject(type, new Image {Source = _imageDictionary[type]},
                            _demensionDictionary[type].Width, _demensionDictionary[type].Height,
                            _demensionDictionary[type].Width * j, _demensionDictionary[type].Height * i));

                        if (isCoin)
                        {
                            type = ObjectType.Coin;
                            playground.Add(new ImmovableObject(type, new Image {Source = _imageDictionary[type]},
                                _demensionDictionary[type].Width, _demensionDictionary[type].Height,
                                _gameValues.TileWidth * j + coinCorrectionWidthPlacement,
                                _gameValues.TileHeight * i + coinCorrectionHeightPlacement, true));
                            isCoin = false;
                        }

                        if (isObstacle)
                        {
                            type = ObjectType.Obstacle;
                            playground.Add(new ImmovableObject(type, new Image {Source = _imageDictionary[type]},
                                _demensionDictionary[type].Width, _demensionDictionary[type].Height,
                                _demensionDictionary[type].Width * j,
                                _gameValues.TileHeight * i + obstacleCorrectionHeightPlacement, false));
                            isObstacle = false;
                        }

                        counter++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return playground;
        }

        public void DrawPlayground(List<IPlaygroundObject> list, Canvas canvas)
        {
            list.ForEach(x =>
            {
                Canvas.SetTop(x.Image, x.Y);
                Canvas.SetLeft(x.Image, x.X);
                canvas.Children.Add(x.Image);
            });
        }
    }
}