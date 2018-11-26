using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfGame;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;
using WpfGame.Editor;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Models.Animations;
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
        private Timer _refreshTimer;
        private Timer _pacmanAnimationTimer;
        private Step _step;
        private Position _position;
        private PacmanAnimation _pacmanAnimation;

        public GameViewController(MainWindow mainWindow, string selectedGame) 
            : base(mainWindow)
        {
            this._selectedGame = selectedGame;
            
            _gameView = new GameView();
            _gameValues = new GameValues();       
            _hitTester = new HitTester(_gameValues);
            _refreshTimer = new Timer{Interval = 16.6667};
            _pacmanAnimationTimer = new Timer{Interval = 150};
            _step = new Step();
            _position = new Position(_gameValues);
            _pacmanAnimation = new PacmanAnimation();

            Canvas = _gameView.GameCanvas;
            
            SetKeyDownEvents(OnButtonKeyDown);
            _gameView.GameCanvas.Loaded += GameCanvas_Loaded;
            _refreshTimer.Elapsed += Refresh_GameCanvas;
            _pacmanAnimationTimer.Elapsed += _pacmanAnimationTimer_Elapsed;
            _mainWindow.Closing += _mainWindow_Closing;

            SetContentOfMain(mainWindow, _gameView);
            _pacmanAnimation.LoadPacmanImages();

        }

        private void _pacmanAnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //so we have to call the dispatcher to grab authority over the GUI
            _gameView.GameCanvas.Dispatcher.Invoke(() =>
            {
                _player.Image.Source = _pacmanAnimation.SetAnimation(_player.CurrentMove);
            });
        }

        private void _mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _pacmanAnimationTimer.Stop();
            _refreshTimer.Stop();
        }

        private void Refresh_GameCanvas(object sender, ElapsedEventArgs e)
        {
            
            //so we have to call the dispatcher to grab authority over the GUI
            _gameView.GameCanvas.Dispatcher.Invoke(() =>
            {
                //we only set the (next) step if the sprite doesnt hit a outerborder nor an obstacle on the nextstep, we set the currentstep again
                //if it succeed the hittest, if it fails we stop the movement
                if (!_hitTester.BorderHit(_player, _player.NextMove) && !_hitTester.ObstacleHit(_tiles, _player, _player.NextMove, x => x.IsWall))
                {
                    _position.UpdatePosition(_player, _player.NextMove);
                    _player.CurrentMove = _player.NextMove;   
                }
                else
                {
                    if (!_hitTester.BorderHit(_player, _player.CurrentMove) && !_hitTester.ObstacleHit(_tiles, _player, _player.CurrentMove, x => x.IsWall))
                    {
                        _position.UpdatePosition(_player, _player.CurrentMove);
                    }
                    else
                    {
                        _player.NextMove = _player.CurrentMove = Move.Stop;
                    }
                }
                _step.SetStep(_player);
            });
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _player.NextMove = Move.Up;
                    break;
                case Key.Down:
                    _player.NextMove = Move.Down;
                    break;
                case Key.Left:
                    _player.NextMove = Move.Left;
                    break;
                case Key.Right:
                    _player.NextMove = Move.Right;
                    break;
                case Key.Escape:
                    new StartWindowViewController(_mainWindow);
                    break;
            }
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

            _tiles = new List<Tile>(new TileRenderer(new JsonPlaygroundParser(_selectedGame).GetOutputList(), _gameValues).GetRenderdTiles());
            LoadTiles(_tiles);


            // there is some minor difference in the x/y and width/size of the tiles and pacman. So we have to correct the size of pacman so that the hittesting
            // will succeed and pacman doenst get stuck on the playingfield
            _player = new Player(Math.Round(_gameValues.TileWidth * 0.89,2), Math.Round(_gameValues.TileHeight * 0.925,2), 0, _gameValues.TileHeight*1.035);


            SpriteRenderer.Draw(_player.X, _player.Y, new Behaviour.Size(20, 20), _player.Image);
            _refreshTimer.Start();
            _pacmanAnimationTimer.Start();
        }


    }
}