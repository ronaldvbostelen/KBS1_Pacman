using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
        private MovableObject _enemy;
        private CollisionDetecter _collisionDetecter;
        private Timer _refreshTimer;
        private Timer _pacmanAnimationTimer;
        private Timer _obstacleTimer;
        private Step _step;
        private Position _position;
        private PacmanAnimation _pacmanAnimation;
        private ObstacleAnimation _obstacleAnimation;
        private Clock _clock;
        private Score _score;
        private const int AmountOfTilesWidth = 20;
        private int hitEndSpotCounter;
        private PlaygroundFactory _playgroundFactory;
        private PlayerFactory _playerFactory;
        private EnemyFactory _enemyFactory;

        public GameViewController(MainWindow mainWindow, string selectedGame) 
            : base(mainWindow)
        {
            _selectedGame = selectedGame;
            
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
            _clock = new Clock();
            _score = new Score();
            _playgroundFactory = new PlaygroundFactory();
            _playerFactory = new PlayerFactory();
            _enemyFactory = new EnemyFactory();

            SetContentOfMain(mainWindow, _gameView);

            Canvas = _gameView.GameCanvas;

            SetKeyDownEvents(_gameView.GameCanvas, OnButtonKeyDown);
            SetKeyUpEvents(_gameView.GameCanvas, OnButtonKeyUp);
            _gameView.GameCanvas.Loaded += GameCanvas_Loaded;
            _refreshTimer.Elapsed += Refresh_GameCanvas;
            _pacmanAnimationTimer.Elapsed += _pacmanAnimationTimer_Elapsed;
            _obstacleTimer.Elapsed += _obstacleTimer_Elapsed;
            _mainWindow.Closing += _mainWindow_Closing;
            _clock.PlaytimeIsOVerEventHander += On_PlaytimeIsOver;


            _pacmanAnimation.LoadPacmanImages();
            _obstacleAnimation.LoadObstacleImages();

            _collisionDetecter.CoinCollision += OnCoinCollision;
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
                //Update clock
                _gameView.GameClockHolder.Text = _clock.Display;
                //Update score
                _gameView.GameScoreHolder.Text = $"Score: {_score.ScoreValue.ToString()}";

                //we only set the (next) step if the sprite doesnt hit a outerborder nor an obstacle on the nextstep, we set the currentstep again
                //if it succeed the hittest, if it fails we stop the movement
                switch (_collisionDetecter.ObjectCollision(_playgroundObjects, _player, _player.NextMove))
                {
                    case Collision.Endpoint:
                        hitEndSpotCounter++;
                        if (hitEndSpotCounter > 15)
                        {
                            FinishGame();
                        }
                        break;
                    case Collision.Enemy:
                    case Collision.Obstacle:
                        EndGame();
                        break;
                    case Collision.Coin:
                    case Collision.Clear:
                        _player.CurrentMove = _player.NextMove;
                        break;
                    case Collision.Border:
                    case Collision.Wall:
                        if (_collisionDetecter.ObjectCollision(_playgroundObjects, _player, _player.CurrentMove) == Collision.Wall ||
                            _collisionDetecter.ObjectCollision(_playgroundObjects, _player, _player.CurrentMove) == Collision.Border)
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
            _gameView.GameClockHolder.Text = _clock.Display; //This is a little hack that prevents the clock from standing still on 00:01 instead of 00:00
            _gameView.TimeIsUpTextBlock.Text = $"Time is up!";
            _gameView.GameTimeIsOverPanel.Visibility = Visibility.Visible;
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
            DisplayScore();
            _obstacleTimer.Stop();
            _pacmanAnimationTimer.Stop();
            _refreshTimer.Stop();
        }

        private void DisplayScore()
        {
            _gameView.TimeIsUpTextBlock.Text = $"Total score: {_score.ScoreValue}";
            _gameView.GameTimeIsOverPanel.Visibility = Visibility.Visible;
            _score.WriteTotalScoreToHighscores();
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

        private void OnButtonKeyUp(object sender, KeyEventArgs e)
        {
            //if the player releases his current key his nextmove will be reset to his current move, this prevents setting the next step in advance.
            _player.NextMove = _player.CurrentMove;
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {

            SetGameValues();
            _gameView.GameCanvas.Focus();

            try
            {
                _playgroundFactory.LoadFactory(_gameValues);
                _playgroundObjects = new List<IPlaygroundObject>(_playgroundFactory.LoadPlayground(new JsonPlaygroundParser(_selectedGame).GetOutputList()));
                _playgroundFactory.DrawPlayground(_playgroundObjects,_gameView.GameCanvas);
                
                _playerFactory.LoadFactory(_gameValues);
                _player = _playerFactory.LoadPlayer(_playgroundObjects);
                _playerFactory.DrawPlayer(_player, _gameView.GameCanvas);

                _enemyFactory.LoadFactory(_gameValues);
                _enemy = _enemyFactory.LoadEnemy(_playgroundObjects);
                _enemyFactory.DrawEnemy(_enemy, _gameView.GameCanvas);

//                LoadPlayer(_playgroundObjects);
//                LoadEnemy(_playgroundObjects);


                _clock.InitializeTimer();
                _refreshTimer.Start();
                _pacmanAnimationTimer.Start();
                _obstacleTimer.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Sorry, something went wrong. Message: " + exception.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                //go back to mainscreen
                new StartWindowViewController(_mainWindow);
            }
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

        private void LoadEnemy(List<IPlaygroundObject> playgroundObjects)
        {
            _enemy = new MovableObject(ObjectType.Enemy, new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Sprites/Enemy/blinky-right.png"))
            },
            // there is some minor difference in the x/y and width/size of the tiles and pacman. So we have to correct the size of pacman so that the hittesting
            // will succeed and pacman doenst get stuck on the playingfield
            _gameValues.TileWidth * .93, _gameValues.TileHeight * .93, 0, _gameValues.TileHeight * 3.04);
            _enemy.X = 197.2357;//niet echt mooi gedaan, nog wijzigen
            _enemy.Y = 283.0037;//niet echt mooi gedaan, nog wijzigen
            Canvas.SetTop(_enemy.Image, _enemy.Y);
            Canvas.SetLeft(_enemy.Image, _enemy.X);
            _gameView.GameCanvas.Children.Add(_enemy.Image);
        }

        /**
         * We used a delegate for the collision with the coin, because it currently
         * isn't possible to return a coin in the CollisionDetecter.
         **/
        public void OnCoinCollision(object sender, ImmovableEventArgs args)
        {
            args.Coin.State = false;
            _gameView.GameCanvas.Children.Remove(args.Coin.Image); //Remove coin from vanvas
            _score.ScoreValue += 10;
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