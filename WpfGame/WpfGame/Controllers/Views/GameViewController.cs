using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfGame;
using WpfGame.Controllers.Renderer;
using WpfGame.Editor;
using WpfGame.Models;
using WpfGame.Values;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class GameViewController : ViewController
    {
        private GameView _gameView;
        private GameValues _gameValues;
        private const int AmountOfTilesWidth = 20;
        private string selectedGame;
        private List<Tile> tiles;

        public GameViewController(MainWindow mainWindow, string selectedGame) : base(mainWindow)
        {
            this.selectedGame = selectedGame;

            
            _gameView = new GameView();
            _gameValues = new GameValues();

            _gameView.GameCanvas.Loaded += GameCanvas_Loaded;
            _gameView.GameCanvas.KeyDown += GameCanvas_KeyDown;

            SetContentOfMain(mainWindow, _gameView);
            
        }

        private void LoadTiles(List<Tile> list)
        {
            list.ForEach(x =>
            {
                Canvas.SetTop(x.Rectangle, x.Y);
                Canvas.SetLeft(x.Rectangle, x.X);
                _gameView.GameCanvas.Children.Add(x.Rectangle);
            });
        }

        private void GameCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    new StartWindowViewController(_mainWindow);
                    break;
            }
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            _gameValues.PlayCanvasHeight = _gameView.GameCanvas.ActualHeight;
            _gameValues.PlayCanvasWidth = _gameView.GameCanvas.ActualWidth;
            _gameValues.HeigthWidthRatio = _gameValues.PlayCanvasHeight / _gameValues.PlayCanvasWidth;
            _gameValues.AmountOfXtiles = AmountOfTilesWidth;
            _gameValues.AmountofYtiles =  Math.Round(_gameValues.AmountOfXtiles * _gameValues.HeigthWidthRatio);
            _gameValues.TileWidth = _gameValues.PlayCanvasWidth / _gameValues.AmountOfXtiles;
            _gameValues.TileHeight = _gameValues.PlayCanvasHeight / _gameValues.AmountofYtiles;
            
            _gameView.GameCanvas.Focus();

            tiles = new List<Tile>(new TileRenderer(new JsonPlaygroundParser(selectedGame).GetOutputList(), _gameValues).GetRenderdTiles());
            LoadTiles(tiles);
        }


    }
}