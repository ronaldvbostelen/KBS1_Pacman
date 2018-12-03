using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
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
        private string _selectedGame;
        private static Random _random;
        private GameState _gameState;
        private GameView _gameView;
        private GameValues _gameValues;
        private Timer _refreshTimer;
        private Timer _pacmanAnimationTimer;
        private Timer _obstacleTimer;
        private Step _step;
        private Position _position;
        private PacmanAnimation _pacmanAnimation;
        private ObstacleAnimation _obstacleAnimation;
        private Clock _clock;
        private Score _score;
        private PlaygroundFactory _playgroundFactory;
        private PlayerFactory _playerFactory;
        private EnemyFactory _enemyFactory;
        private List<IPlaygroundObject> _playgroundObjects;
        private MovableObject _player;
        private MovableObject _enemy;

        //test
        private Timer _steps;


        public GameViewController(MainWindow mainWindow, string selectedGame)
            : base(mainWindow)
        {
            _refreshTimer = new Timer {Interval = 1000 / 60};
            _pacmanAnimationTimer = new Timer {Interval = 150};
            _obstacleTimer = new Timer {Interval = 3000};
            _clock = new Clock();
            _score = new Score();
            _gameView = new GameView();
            _gameValues = new GameValues();
            _playgroundFactory = new PlaygroundFactory();
            _playerFactory = new PlayerFactory();
            _enemyFactory = new EnemyFactory();
            _pacmanAnimation = new PacmanAnimation();
            _obstacleAnimation = new ObstacleAnimation();
            _step = new Step();
            _position = new Position(_gameValues);
            _random = new Random();

            //test
            _steps = new Timer {Interval = 2000};
            _steps.Elapsed += _steps_Elapsed;

            SetContentOfMain(mainWindow, _gameView);

            _selectedGame = selectedGame;
            _gameState = GameState.Playing;

            SetKeyDownEvents(_gameView.GameCanvas, OnButtonKeyDown);
            SetKeyUpEvents(_gameView.GameCanvas, OnButtonKeyUp);
            _gameView.GameCanvas.Loaded += OnGameCanvasLoaded;
            _refreshTimer.Elapsed += RefreshGameCanvas;
            _pacmanAnimationTimer.Elapsed += OnPacmanAnimationTimerElapsed;
            _obstacleTimer.Elapsed += OnObstacleTimerElapsed;
            _mainWindow.Closing += OnMainWindowClosing;
            _clock.PlaytimeIsOver += OnPlaytimeIsOver;

            _position.CollisionDetecter.CoinCollision += OnCoinCollision;
            _position.CollisionDetecter.EndpointCollision += OnEndpointCollision;
            _position.CollisionDetecter.EnemyCollision += OnOnEnemyCollision;
            _position.CollisionDetecter.ObstacleCollision += OnObstacleCollision;

            _pacmanAnimation.LoadPacmanImages();
            _obstacleAnimation.LoadObstacleImages();
        }

        private void _steps_Elapsed(object sender, ElapsedEventArgs e)
        {
            _enemy.NextMove = (Move) _random.Next(1, 5);
        }

        private void OnObstacleCollision(object sender, EventArgs e)
        {
            _gameState = GameState.Lost;
        }

        private void OnOnEnemyCollision(object sender, EventArgs e)
        {
            _gameState = GameState.Lost;
        }

        private void OnEndpointCollision(object sender, EventArgs e)
        {
            _gameState = GameState.Finished;
        }

        private void OnObstacleTimerElapsed(object sender, ElapsedEventArgs e)
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

        private void OnPacmanAnimationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //so we have to call the dispatcher to grab authority over the GUI
            _gameView.GameCanvas.Dispatcher.Invoke(() =>
            {
                _player.Image.Source = _pacmanAnimation.SetAnimation(_player.CurrentMove);
            });
        }

        private void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _obstacleTimer.Stop();
            _pacmanAnimationTimer.Stop();
            _refreshTimer.Stop();
        }

        private void RefreshGameCanvas(object sender, ElapsedEventArgs e)
        {
            //so we have to call the dispatcher to grab authority over the GUI
            _gameView.GameCanvas.Dispatcher.Invoke(() =>
            {
                //Update clock
                _gameView.GameClockHolder.Text = _clock.Display;
                //Update score
                _gameView.GameScoreHolder.Text = $"Score: {_score.ScoreValue.ToString()}";

                //Update playerposition based on userinput
                _position.ProcessMove(_player);
                _step.SetStep(_player);

                //test
                _position.ProcessMove(_enemy);
                _step.SetStep(_enemy);

                //Validate gamestate
                ValidateGamestate();
            });
        }

        private void ValidateGamestate()
        {
            switch (_gameState)
            {
                case GameState.Finished:
                    FinishGame();
                    break;
                case GameState.Lost:
                    EndGame();
                    break;
                case GameState.OutOfTime:
                    StopGame();
                    break;
            }
        }

        private void StopGame()
        {
            _gameView.GameClockHolder.Text =
                _clock.Display; //This is a little hack that prevents the clock from standing still on 00:01 instead of 00:00
            _gameView.TimeIsUpTextBlock.Text = $"Time is up!";
            _gameView.GameTimeIsOverPanel.Visibility = Visibility.Visible;
            EndGame();
        }

        private void OnPlaytimeIsOver(object sender, EventArgs e)
        {
            _gameState = GameState.OutOfTime;
        }

        private void EndGame()
        {
            _gameView.EndGamePanel.Visibility = Visibility.Visible;
            StopTimers();
        }

        private void FinishGame()
        {
            _gameView.FinishGamePanel.Visibility = Visibility.Visible;
            DisplayScore();
            StopTimers();
        }

        private void StopTimers()
        {
            _obstacleTimer.Stop();
            _pacmanAnimationTimer.Stop();
            _refreshTimer.Stop();
            _clock.StopClock();
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

        private void OnGameCanvasLoaded(object sender, RoutedEventArgs e)
        {
            var somethingWentWrongWhenLoadingTheGame = true;
            SetGameValues();
            _gameView.GameCanvas.Focus();

            try
            {
                _playgroundFactory.LoadFactory(_gameValues);
                //load the playground off the selected jsonfile
                _playgroundObjects = new List<IPlaygroundObject>(
                    _playgroundFactory.LoadPlayground(new JsonPlaygroundParser(_selectedGame).GetOutputList()));
                _playgroundFactory.DrawPlayground(_playgroundObjects, _gameView.GameCanvas);

                _enemyFactory.LoadFactory(_gameValues);
                _enemy = _enemyFactory.LoadEnemy(_playgroundObjects);
                _enemyFactory.DrawEnemy(_enemy, _gameView.GameCanvas);

                _playerFactory.LoadFactory(_gameValues);
                _player = _playerFactory.LoadPlayer(_playgroundObjects);
                _playerFactory.DrawPlayer(_player, _gameView.GameCanvas);

                //add enemy and player to playgroundobjectsList
                _playgroundObjects.Add(_enemy);
                _playgroundObjects.Add(_player);

                _position.PlaygroundObjects = _playgroundObjects;

                _clock.InitializeTimer();
                _refreshTimer.Start();
                _pacmanAnimationTimer.Start();
                _obstacleTimer.Start();
                _steps.Start();

                somethingWentWrongWhenLoadingTheGame = false;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(
                    "Your root folder does not contain a Playgrounds folder. Unable to load a playground. ",
                    "Playgrounds folder not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(
                    "Your Playgrounds folder doesn't contain a file. Please create or download a playground.",
                    "Playgrounds folder empty", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Could not read the file. Please check if the file is corrupt. ",
                    "Unable to read file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Your JSON-file is invalid. Please load another file or repair the current.",
                    "Invalid JSON", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Your JSON-file is incomplete. Please load another file or repair the current",
                    "Incomplete JSON-file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Something went horribly wrong. Please contact your software-supplier." + ex.Message + " " +
                    ex.StackTrace,
                    "Help", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (somethingWentWrongWhenLoadingTheGame)
            {
                //go back to mainscreen
                new StartWindowViewController(_mainWindow);
            }
        }


        /**
         * We used a delegate for the collision with the coin, because it currently
         * isn't possible to return a coin in the CollisionDetecter.
         **/
        private void OnCoinCollision(object sender, ImmovableEventArgs args)
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
            _gameValues.AmountOfXtiles = 20;
            _gameValues.AmountofYtiles = Math.Round(_gameValues.AmountOfXtiles * _gameValues.HeigthWidthRatio);
            _gameValues.TileWidth = _gameValues.PlayCanvasWidth / _gameValues.AmountOfXtiles;
            _gameValues.TileHeight = _gameValues.PlayCanvasHeight / _gameValues.AmountofYtiles;
            _gameValues.Movement = 2.5;
        }
    }
}