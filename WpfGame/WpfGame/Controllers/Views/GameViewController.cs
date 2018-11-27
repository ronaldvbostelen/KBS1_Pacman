using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Random _random;
        private GameView _gameView;
        private GameValues _gameValues;
        private string _selectedGame;
        private List<Tile> _tiles;
        private List<Obstacle> _obstacles;
        private List<Coin> _coins;
        private Player _player;
        private CollisionDetecter _hitTester;
        private Timer _refreshTimer;
        private Timer _pacmanAnimationTimer;
        private Timer _obstacleTimer;
        private Step _step;
        private Position _position;
        private PacmanAnimation _pacmanAnimation;
        private const int AmountOfTilesWidth = 20;

        public GameViewController(MainWindow mainWindow, string selectedGame) 
            : base(mainWindow)
        {
            this._selectedGame = selectedGame;
            
            _gameView = new GameView();
            _gameValues = new GameValues();       
            _hitTester = new CollisionDetecter(_gameValues);
            _refreshTimer = new Timer{Interval = 16.6667};
            _pacmanAnimationTimer = new Timer{Interval = 150};
            _obstacleTimer = new Timer{Interval = 3000};
            _step = new Step();
            _position = new Position(_gameValues);
            _pacmanAnimation = new PacmanAnimation();
            _obstacles = new List<Obstacle>();
            _coins = new List<Coin>();
            _random = new Random();

            Canvas = _gameView.GameCanvas;
            
            SetKeyDownEvents(OnButtonKeyDown);
            _gameView.GameCanvas.Loaded += GameCanvas_Loaded;
            _refreshTimer.Elapsed += Refresh_GameCanvas;
            _pacmanAnimationTimer.Elapsed += _pacmanAnimationTimer_Elapsed;
            _obstacleTimer.Elapsed += _obstacleTimer_Elapsed;
            _mainWindow.Closing += _mainWindow_Closing;

            SetContentOfMain(mainWindow, _gameView);
            _pacmanAnimation.LoadPacmanImages();

        }

        private void _obstacleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _gameView.GameCanvas.Dispatcher.Invoke(() =>
            {

                foreach (var obstacle in _obstacles)
                {
                    if (!obstacle.IsEnabled && _random.NextDouble() > 0.5)
                    {
                        obstacle.IsEnabled = true;
                        obstacle.Rectangle.Fill = Brushes.Black;
                    }
                    else
                    {
                        obstacle.IsEnabled = false;
                        obstacle.Rectangle.Fill = Brushes.White;
                    }
                }
            });
            
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
            _obstacleTimer.Stop();
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
                if (!_hitTester.BorderCollision(_player, _player.NextMove) && !_hitTester.ObjectCollision(_tiles, _player, _player.NextMove, x => x.IsWall))
                {
                    //next we check wether our player has hit an enabled obstacle
                    if (!_hitTester.ObjectCollision(_obstacles, _player, _player.NextMove, x => x.IsEnabled))
                    {
                        if (_hitTester.ObjectCollision(_tiles, _player, _player.CurrentMove, x => x.IsEnd))
                        {
                            FinishGame();
                        }
                        _position.UpdatePosition(_player, _player.NextMove);
                        _player.CurrentMove = _player.NextMove;
                    }
                    else
                    {
                        //PLAYER IS DEAD.
                        EndGame();
                    }

                }
                else
                {
                    if (!_hitTester.BorderCollision(_player, _player.CurrentMove) && !_hitTester.ObjectCollision(_tiles, _player, _player.CurrentMove, x => x.IsWall))
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

        private void EndGame()
        {
            _gameView.EndGamePanel.Visibility = Visibility.Visible;
            _obstacleTimer.Stop();
            _pacmanAnimationTimer.Stop();
            _refreshTimer.Stop();
        }

        private void FinishGame()
        {
            _gameView.FinishGamePanel.Visibility = Visibility.Visible;
            _obstacleTimer.Stop();
            _pacmanAnimationTimer.Stop();
            _refreshTimer.Stop();
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
        
        private void LoadObjects<T>(List<T> list) where T : PlaygroundObject
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
            
            //create obstacleliste based on the tileslist
            _tiles.Where(x => x.HasObstacle).ToList().ForEach(x =>
                _obstacles.Add(new Obstacle(x.X + (x.Rectangle.Width * 0.1), x.Y + (x.Rectangle.Height * 0.1), x.Rectangle.Width * 0.8, x.Rectangle.Height * 0.8)));

            _tiles.Where(x => x.HasCoin).ToList().ForEach(x =>
                _coins.Add(new Coin(x.X + (x.Rectangle.Width * 0.1), x.Y + (x.Rectangle.Height * 0.1), x.Rectangle.Width * 0.8, x.Rectangle.Height * 0.8)));


            LoadObjects(_tiles);
            LoadObjects(_obstacles);
            LoadObjects(_coins);

            // there is some minor difference in the x/y and width/size of the tiles and pacman. So we have to correct the size of pacman so that the hittesting
            // will succeed and pacman doenst get stuck on the playingfield
            _player = new Player(_gameValues.TileWidth * 0.895, _gameValues.TileHeight * 0.935, 0, _gameValues.TileHeight*1.035);

            SpriteRenderer.Draw(_player.X, _player.Y, new Behaviour.Size(20, 20), _player.Image);
            _refreshTimer.Start();
            _pacmanAnimationTimer.Start();
            _obstacleTimer.Start();
        }

    }
}