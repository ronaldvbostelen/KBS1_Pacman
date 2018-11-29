using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;
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
        private List<IPlaygroundObject> _playgroundObjects;
        private MovableObject _player;
        private CollisionDetecter _collisionDetecter;
        private Timer _refreshTimer;
        private Timer _pacmanAnimationTimer;
        private Timer _obstacleTimer;
        private Step _step;
        private Position _position;
        private PacmanAnimation _pacmanAnimation;
        private ObstacleAnimation _obstacleAnimation;
        private ClockController _clockController;
        private const int AmountOfTilesWidth = 20;
        private int hitEndSpotCounter;

        public GameViewController(MainWindow mainWindow, string selectedGame) 
            : base(mainWindow)
        {
            this._selectedGame = selectedGame;
            
            _gameView = new GameView();
            _gameValues = new GameValues();       
            _collisionDetecter = new CollisionDetecter(_gameValues);
            _refreshTimer = new Timer { Interval = 1000/60 };
            _pacmanAnimationTimer = new Timer{Interval = 150};
            _obstacleTimer = new Timer{Interval = 3000};
            _step = new Step();
            _position = new Position(_gameValues);
            _pacmanAnimation = new PacmanAnimation();
            _obstacleAnimation = new ObstacleAnimation();
            _random = new Random();
            _clockController = new ClockController();
            

            Canvas = _gameView.GameCanvas;
            
            SetKeyDownEvents(OnButtonKeyDown);
            _gameView.GameCanvas.Loaded += GameCanvas_Loaded;
            _refreshTimer.Elapsed += Refresh_GameCanvas;
            _pacmanAnimationTimer.Elapsed += _pacmanAnimationTimer_Elapsed;
            _obstacleTimer.Elapsed += _obstacleTimer_Elapsed;
            _mainWindow.Closing += _mainWindow_Closing;
            _clockController.PlaytimeIsOVerEventHander += On_PlaytimeIsOver;

            SetContentOfMain(mainWindow, _gameView);
            _pacmanAnimation.LoadPacmanImages();
            _obstacleAnimation.LoadObstacleImages();
        }

        private void _obstacleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _gameView.GameCanvas.Dispatcher.Invoke(() =>
            {
                foreach (var obj in _playgroundObjects)
                {
                    if (obj.ObjectType == ObjectType.Obstacle)
                    {
                        var obst = (ImmovableObject) obj;
                        
                        if (!obst.State && _random.NextDouble() > 0.4)
                        {
                            obst.Image.Source = _obstacleAnimation.SetObstacleImage(obst.State = true);
                        }
                        else
                        {
                            obst.Image.Source = _obstacleAnimation.SetObstacleImage(obst.State = false);
                        }
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
//                updateclock
                _gameView.GameClockHolder.Text = _clockController.Display;

                //we only set the (next) step if the sprite doesnt hit a outerborder nor an obstacle on the nextstep, we set the currentstep again
                //if it succeed the hittest, if it fails we stop the movement
                switch (_collisionDetecter.ObjectCollision(_playgroundObjects, _player, _player.NextMove))
                {
                    case NextStep.Endpoint:
                        hitEndSpotCounter++;
                        if (hitEndSpotCounter > 15)
                        {
                            FinishGame();
                        }
                        break;
                    case NextStep.Enemy:
                    case NextStep.Obstacle:
                        EndGame();
                        break;
                    case NextStep.Coin:
                    case NextStep.Clear:
                        _player.CurrentMove = _player.NextMove;
                        break;
                    case NextStep.Border:
                    case NextStep.Wall:
                        if (_collisionDetecter.ObjectCollision(_playgroundObjects, _player, _player.CurrentMove) == NextStep.Wall ||
                            _collisionDetecter.ObjectCollision(_playgroundObjects, _player, _player.CurrentMove) == NextStep.Border)
                        {
                            _player.CurrentMove = Move.Stop;
                        }

                        break;
                }
                _position.UpdatePosition(_player);
                _step.SetStep(_player);
            });
        }

        public void On_PlaytimeIsOver(object sender, EventArgs e)
        {
            var totalscore = 10000;
            _gameView.GameClockHolder.Text = _clockController.Display;
            _gameView.TimeIsUpTextBlock.Text = $"Time's up! Total score: {totalscore}";
            _gameView.GameTimeIsOVerPanel.Visibility = Visibility.Visible;
            EndGame();
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
        
        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetGameValues();
            _gameView.GameCanvas.Focus();

            _playgroundObjects = new List<IPlaygroundObject>(new TileRenderer(new JsonPlaygroundParser(_selectedGame).GetOutputList(), _gameValues).RenderTiles());
            LoadObjects(_playgroundObjects);
            LoadPlayer(_playgroundObjects);
            
            _clockController.InitializeTimer();
            _refreshTimer.Start();
            _pacmanAnimationTimer.Start();
            _obstacleTimer.Start();
        }

        private void LoadObjects(List<IPlaygroundObject> list)
        {
            list.ForEach(x =>
            {
                Canvas.SetTop(x.Image, x.Y);
                Canvas.SetLeft(x.Image, x.X);
                _gameView.GameCanvas.Children.Add(x.Image);
            });
        }
        
        private void LoadPlayer(List<IPlaygroundObject> playgroundObjects)
        {
            _player = new MovableObject(ObjectType.Player, new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Pacman/pacman-right-halfopenjaw.png"))
            },
                // there is some minor difference in the x/y and width/size of the tiles and pacman. So we have to correct the size of pacman so that the hittesting
                // will succeed and pacman doenst get stuck on the playingfield
            _gameValues.TileWidth * .93, _gameValues.TileHeight * .93, 0, _gameValues.TileHeight * 3.04);
            Canvas.SetTop(_player.Image, _player.Y);
            Canvas.SetLeft(_player.Image, _player.X);
            _gameView.GameCanvas.Children.Add(_player.Image);
        }

        private void SetGameValues()
        {
            _gameValues.PlayCanvasHeight = _gameView.GameCanvas.ActualHeight;
            _gameValues.PlayCanvasWidth = _gameView.GameCanvas.ActualWidth;
            _gameValues.HeigthWidthRatio = _gameValues.PlayCanvasHeight / _gameValues.PlayCanvasWidth;
            _gameValues.AmountOfXtiles = AmountOfTilesWidth;
            _gameValues.AmountofYtiles = Math.Round(_gameValues.AmountOfXtiles * _gameValues.HeigthWidthRatio);
            _gameValues.TileWidth = _gameValues.PlayCanvasWidth / _gameValues.AmountOfXtiles;
            _gameValues.TileHeight = _gameValues.PlayCanvasHeight / _gameValues.AmountofYtiles;
            _gameValues.Movement = 2.5;
        }

    }
}