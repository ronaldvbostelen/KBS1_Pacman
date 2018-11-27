using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;

namespace WpfGame.Controllers.Renderer
{
    public class TileRenderer
    {
        private List<IPlaygroundObject> _playgroundObjects;
        private List<TileMockup> _mockups;
        private GameValues _gameValues;

        public TileRenderer(List<TileMockup> mockups, GameValues gameValues)
        {
            _gameValues = gameValues;
            _mockups = mockups;
        }

        public List<IPlaygroundObject> RenderTiles()
        {
            List<IPlaygroundObject> outList = new List<IPlaygroundObject>();
            int counter = 0;

            for (int i = 0; i < _gameValues.AmountofYtiles; i++)
            {
                for (int j = 0; j < _gameValues.AmountOfXtiles; j++)
                {
                    if (_mockups[counter].IsSpawn)
                    {
                        outList.Add(new StaticObject(ObjectType.SpawnPoint,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/spawn.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight));
                    }
                    else if (_mockups[counter].IsEnd)
                    {
                        outList.Add(new StaticObject(ObjectType.EndPoint,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/end.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight));
                    }
                    else if (_mockups[counter].IsStart)
                    {
                        outList.Add(new StaticObject(ObjectType.StartPoint,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/floor.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight));
                    }
                    else if (_mockups[counter].IsWall)
                    {
                        outList.Add(new StaticObject(ObjectType.Wall,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/wall.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight));
                    }
                    else if (_mockups[counter].HasCoin)
                    {
                        outList.Add(new StaticObject(ObjectType.Path,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/floor.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight));
                        outList.Add(new ImmovableObject(ObjectType.Coin,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/coin.png"))
                            },
                            _gameValues.TileWidth * 0.65, _gameValues.TileHeight * 0.65, j * _gameValues.TileWidth + (_gameValues.TileWidth * 0.17),
                            i * _gameValues.TileHeight + (_gameValues.TileHeight * 0.17),true));
                    }
                    else if (_mockups[counter].HasObstacle)
                    {
                        outList.Add(new StaticObject(ObjectType.Path,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/floor.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight));
                        outList.Add(new ImmovableObject(ObjectType.Obstacle,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/ObstacleOff.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight,false));
                    }
                    else
                    {
                        outList.Add(new StaticObject(ObjectType.Path,
                            new Image
                            {
                                Source = new BitmapImage(
                                    new Uri("pack://application:,,,/Assets/Sprites/Objects/floor.png"))
                            },
                            _gameValues.TileWidth, _gameValues.TileHeight, j * _gameValues.TileWidth,
                            i * _gameValues.TileHeight));
                    }
                    counter++;
                }
            }

            return outList;
        }
    }
}