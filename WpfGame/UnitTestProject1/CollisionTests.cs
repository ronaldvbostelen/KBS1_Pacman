using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Internal;
using WpfGame;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Creatures;
using WpfGame.Controllers.Renderer;
using WpfGame.Controllers.Views;
using WpfGame.Editor;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Values;
using WpfGame.Views;
using Assert = NUnit.Framework.Assert;

namespace WpfGame.UnitTests
{
    [TestFixture(Description = "Collision tests")]
    public class CollisionTests
    {
        private GameValues _gameValues;
        private MovableObject _player;
        private PlaygroundFactory _playgroundFactory;
        private List<IPlaygroundObject> _playgroundObjects;
        private CollisionDetecter _collisionDetecter;
        private GameState _gameState;

        public CollisionTests()
        {
            //this piece of code gave me headache. you'll need it in every testclass. // NEVER REMOVE IT //
            if (Application.ResourceAssembly == null)
            {
                Application.ResourceAssembly = typeof(MainWindow).Assembly;
            }

            _gameValues = new GameValues();
            _gameValues.PlayCanvasHeight = 600;
            _gameValues.PlayCanvasWidth = 784;
            _gameValues.HeigthWidthRatio = _gameValues.PlayCanvasHeight / _gameValues.PlayCanvasWidth;
            _gameValues.AmountOfXtiles = 20;    
            _gameValues.AmountofYtiles = Math.Round(_gameValues.AmountOfXtiles * _gameValues.HeigthWidthRatio);
            _gameValues.TileWidth = _gameValues.PlayCanvasWidth / _gameValues.AmountOfXtiles;
            _gameValues.TileHeight = _gameValues.PlayCanvasHeight / _gameValues.AmountofYtiles;
            _gameValues.Movement = 2.5;

            _playgroundFactory = new PlaygroundFactory();
            _playgroundFactory.LoadFactory(_gameValues);

            _player = new MovableObject(ObjectType.Player,
                new Image
                {
                    Source =
                        new BitmapImage(
                            new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
                }, 50, 50, 10, 10);

            _collisionDetecter = new CollisionDetecter(_gameValues);
            _playgroundObjects = new List<IPlaygroundObject>(_playgroundFactory.LoadPlayground(new JsonPlaygroundParser("Playgroundv3.json").GetOutputList()));
            _gameState = GameState.Playing;
        }


        
    }
}


