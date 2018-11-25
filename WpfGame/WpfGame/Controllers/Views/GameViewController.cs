using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfGame;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;
using WpfGame.Editor;
using WpfGame.Models;
using WpfGame.Values;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class GameViewController : ViewController
    {
        public static Canvas Canvas;
        private GameView _gameView;
        private GameValues _gameValues;
        private const int AmountOfTilesWidth = 20;
        private string _selectedGame;
        private List<Tile> _tiles;
        private Player _player;

        public GameViewController(MainWindow mainWindow, string selectedGame) 
            : base(mainWindow)
        {
            this._selectedGame = selectedGame;
            
            _gameView = new GameView();
            _gameValues = new GameValues();       

            Canvas = _gameView.GameCanvas;

            _player = new Player();
            SpriteRenderer.Draw(new Position(_player.X, _player.Y), new Behaviour.Size(20, 20), @"\Assets\Sprites\Pacman\pacman-left-halfopenjaw.png");

            SetKeyDownEvents(OnButtonKeyDown);
            _gameView.GameCanvas.Loaded += GameCanvas_Loaded;
            _gameView.GameCanvas.KeyDown += GameCanvas_KeyDown;

            SetContentOfMain(mainWindow, _gameView);
            
        }

        protected void SetKeyDownEvents(KeyEventHandler e)
        {
            _mainWindow.KeyDown += e;
        }

        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            string imageUri = string.Empty;

            switch (e.Key)
            {
                case Key.Down:
                    _player.Y += 25;
                    imageUri = @"\Assets\Sprites\Pacman\pacman-down-halfopenjaw.png";
                    break;
                case Key.Up:
                    _player.Y -= 25;
                    imageUri = @"\Assets\Sprites\Pacman\pacman-up-halfopenjaw.png";
                    break;
                case Key.Left:
                    _player.X -= 25;
                    imageUri = @"\Assets\Sprites\Pacman\pacman-left-halfopenjaw.png";
                    break;
                case Key.Right:
                    _player.X += 25;
                    imageUri = @"\Assets\Sprites\Pacman\pacman-right-halfopenjaw.png";
                    break;
            }

            Image playerImage = SpriteRenderer.GetSpriteImage(imageUri);
            Step.SetStep(playerImage, _player.Y, _player.X);
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

            _tiles = new List<Tile>(new TileRenderer(new JsonPlaygroundParser(_selectedGame).GetOutputList(), _gameValues).GetRenderdTiles());
            LoadTiles(_tiles);
        }


    }
}