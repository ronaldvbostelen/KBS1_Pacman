﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Xaml;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Game;
using WpfGame.Controllers.Renderers;
using WpfGame.Generals;
using WpfGame.Models;
using WpfGame.Models.Playgroundobjects;
using WpfGame.Models.Visuals.Animations;
using WpfGame.Sounds;
using WpfGame.Tooling;
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
        private Timer _spawnTimer;
        private Step _step;
        private Position _position;
        private PacmanAnimation _pacmanAnimation;
        private EnemyAnimation _enemyAnimation;
        private ObstacleAnimation _obstacleAnimation;
        private Clock _clock;
        private Score _score;
        private PlaygroundFactory _playgroundFactory;
        private PlayerFactory _playerFactory;
        private EnemyFactory _enemyFactory;
        private List<IPlaygroundObject> _playgroundObjects;
        private MovableObject _player;
        private Sound _sound;

        public GameViewController(MainWindow mainWindow, string selectedGame)
            : base(mainWindow)
        {
            _refreshTimer = new Timer {Interval = 1000 / 60};
            _pacmanAnimationTimer = new Timer {Interval = 150};
            _obstacleTimer = new Timer {Interval = 3000};
            _spawnTimer = new Timer {Interval = 2500};
            _clock = new Clock();
            _score = new Score();
            _gameView = new GameView();
            _gameValues = new GameValues();
            _playgroundFactory = new PlaygroundFactory();
            _playerFactory = new PlayerFactory();
            _enemyFactory = new EnemyFactory();
            _pacmanAnimation = new PacmanAnimation();
            _enemyAnimation = new EnemyAnimation();
            _obstacleAnimation = new ObstacleAnimation();
            _step = new Step();
            _position = new Position(_gameValues);
            _random = new Random();
            _sound = new Sound();

            SetContentOfMain(mainWindow, _gameView);

            _selectedGame = selectedGame;
            _gameState = GameState.Playing;

            SetKeyDownEvents(_gameView.GameCanvas, OnButtonKeyDown);
            SetKeyUpEvents(_gameView.GameCanvas, OnButtonKeyUp);
            _gameView.GameCanvas.Loaded += OnGameCanvasLoaded;
            MainWindow.Closing += OnMainWindowClosing;
            _refreshTimer.Elapsed += RefreshGameCanvas;
            _pacmanAnimationTimer.Elapsed += OnPacmanAnimationTimerElapsed;
            _obstacleTimer.Elapsed += OnObstacleTimerElapsed;
            _spawnTimer.Elapsed += OnspawnTimerElapsed;
            _clock.PlaytimeIsOver += OnPlaytimeIsOver;
            _clock.PlaytimeIsOver += _sound.OnPlaytimeIsOver;

            _position.CollisionDetecter.CoinCollision += OnCoinCollision;
            _position.CollisionDetecter.CoinCollision += _sound.OnCoinCollision;
            _position.CollisionDetecter.EndpointCollision += OnEndpointCollision;
            _position.CollisionDetecter.EndpointCollision += _sound.OnEndpointCollision;
            _position.CollisionDetecter.EnemyCollision += OnOnEnemyCollision;
            _position.CollisionDetecter.EnemyCollision += _sound.OnOnEnemyCollision;
            _position.CollisionDetecter.ObstacleCollision += OnObstacleCollision;
            _position.CollisionDetecter.ObstacleCollision += _sound.OnObstacleCollision;

            _pacmanAnimation.LoadPacmanImages();
            _enemyAnimation.LoadPacmanImages();
            _obstacleAnimation.LoadObstacleImages();
        }

        private void OnspawnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_random.NextDouble() > .45 && _playgroundObjects.Count(x => x.ObjectType == ObjectType.Enemy) < 5)
            {
                //because we add a new enemy to the canvas we have to call the canvas dispatcher
                _gameView.GameCanvas.Dispatcher.Invoke(() =>
                {
                    _playgroundObjects.Add(_enemyFactory.DrawEnemy(_enemyFactory.LoadEnemy(_playgroundObjects),
                        _gameView.GameCanvas));
                });
            }
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
                //set obstacle animation and state
                _playgroundObjects.Where(x => x.ObjectType == ObjectType.Obstacle).Cast<ImmovableObject>().ToList()
                    .ForEach(x =>
                        x.Image.Source =
                            _obstacleAnimation.SetObstacleImage(x.State = _random.NextDouble() > 0.4 && !x.State));
            });
        }

        private void OnPacmanAnimationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //so we have to call the dispatcher to grab authority over the GUI
            _gameView.GameCanvas.Dispatcher.Invoke(() =>
            {
                //set playeranimation
                _player.Image.Source = _pacmanAnimation.SetAnimation(_player.CurrentMove);

                //set enemyanimation
                _playgroundObjects.Where(x => x.ObjectType == ObjectType.Enemy).Cast<MovableObject>().ToList().ForEach(x =>
                {
                    x.Image.Source = _enemyAnimation.SetAnimation(x.CurrentMove);
                });
            });
        }

        private void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopTimers();
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

                //set and update enemyposition
                _playgroundObjects.Where(x => x.ObjectType == ObjectType.Enemy).ToList().ForEach(x =>
                {
                    _position.EnemyProcessMove((MovableObject) x);
                    _position.ProcessMove((MovableObject) x);
                    _step.SetStep((MovableObject) x);
                });

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
            _gameView.TimeIsUpTextBlock.Text = "Time is up!";
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
            _spawnTimer.Stop();
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
                    new StartWindowViewController(MainWindow);
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
                _playerFactory.LoadFactory(_gameValues);
                _enemyFactory.LoadFactory(_gameValues);

                //load the playground off the selected jsonfile and set reference to _playgroundsobjects
                _playgroundObjects = _playgroundFactory.DrawPlayground(new List<IPlaygroundObject>(
                        _playgroundFactory.LoadPlayground(new JsonPlaygroundParser(_selectedGame).GetOutputList())),
                    _gameView.GameCanvas);

                _player = _playerFactory.DrawPlayer(_playerFactory.LoadPlayer(_playgroundObjects),
                    _gameView.GameCanvas);

                //add player to playgroundobjectsList   
                _playgroundObjects.Add(_player);

                //set the playgroundsobject lets for the positionclass
                _position.PlaygroundObjects = _playgroundObjects;

                //start clocks
                _clock.InitializeTimer();
                _refreshTimer.Start();
                _pacmanAnimationTimer.Start();
                _obstacleTimer.Start();
                _spawnTimer.Start();

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
                new StartWindowViewController(MainWindow);
            }
        }


        /**
         * We used a delegate for the collision with the coin, because it currently
         * isn't possible to return a coin in the CollisionDetecter.
         **/
        private void OnCoinCollision(object sender, ImmovableEventArgs args)
        {
            args.Coin.State = false;
            _gameView.GameCanvas.Children.Remove(args.Coin.Image); //Remove coin from canvas
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