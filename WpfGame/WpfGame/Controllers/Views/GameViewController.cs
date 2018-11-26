using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfGame;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;
using WpfGame.Editor;
using WpfGame.Generals;
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
        private HitTester _hitTester;

        public GameViewController(MainWindow mainWindow, string selectedGame) 
            : base(mainWindow)
        {
            this._selectedGame = selectedGame;
            
            _gameView = new GameView();
            _gameValues = new GameValues();       
            _hitTester = new HitTester(_gameValues);

            Canvas = _gameView.GameCanvas;
            
            SetKeyDownEvents(OnButtonKeyDown);
            _gameView.GameCanvas.Loaded += GameCanvas_Loaded;
            _gameView.GameCanvas.KeyDown += GameCanvas_KeyDown;

            SetContentOfMain(mainWindow, _gameView);
            
        }

        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (_hitTester.HitTopBorderOfPlayground(_player.Y, _gameValues.UpDownMovement))
                    {
                        if (!_hitTester.HitMeBabyHitMeBabyOneMoreTime(_tiles, _player, NextMove.Up, x => x.IsWall))
                        {
                            _player.Y -= _gameValues.UpDownMovement;
                            _player.PlayerImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-up-halfopenjaw.png"));
                        }
                    }
                    break;
                case Key.Down:
                    if (_hitTester.HitBottomBorderOfPlayground(_gameValues.PlayCanvasHeight, _player.PlayerImage.Height, _player.Y, _gameValues.UpDownMovement))
                    {
                        if (!_hitTester.HitMeBabyHitMeBabyOneMoreTime(_tiles, _player, NextMove.Down, x => x.IsWall))
                        {
                            _player.Y += _gameValues.UpDownMovement;
                            _player.PlayerImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-down-halfopenjaw.png"));
                        }
                    }
                    break;
                case Key.Left:
                    if (_hitTester.HitLeftBorderOfPlayground(_player.X, _gameValues.LeftRightMovement))
                    {
                        if (!_hitTester.HitMeBabyHitMeBabyOneMoreTime(_tiles, _player, NextMove.Left, x => x.IsWall))
                        {
                            _player.X -= _gameValues.LeftRightMovement;
                            _player.PlayerImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-left-halfopenjaw.png"));
                        }
                    }
                    break;
                case Key.Right:
                    if (_hitTester.HitRightBorderOfPlayground(_gameValues.PlayCanvasWidth, _player.PlayerImage.Width, _player.X, _gameValues.LeftRightMovement))
                    {
                        if (!_hitTester.HitMeBabyHitMeBabyOneMoreTime(_tiles, _player, NextMove.Right, x => x.IsWall))
                        {
                            _player.X += _gameValues.LeftRightMovement;
                            _player.PlayerImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"));
                        }
                    }
                    break;
            }
            
            Step.SetStep(_player.PlayerImage, _player.Y, _player.X);
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
            _gameValues.UpDownMovement = _gameValues.PlayCanvasHeight / 200;
            _gameValues.LeftRightMovement = _gameValues.PlayCanvasWidth / 200;

            _gameView.GameCanvas.Focus();

            // 0.925/1.04 is magic correction number for collision later on
            _player = new Player(_gameValues.TileWidth * 0.895,_gameValues.TileHeight * 0.925,0,_gameValues.TileHeight * 1.04);

            _tiles = new List<Tile>(new TileRenderer(new JsonPlaygroundParser(_selectedGame).GetOutputList(), _gameValues).GetRenderdTiles());
            LoadTiles(_tiles);

            SpriteRenderer.Draw(_player.X, _player.Y, new Behaviour.Size(20, 20), _player.PlayerImage);
        }


    }
}