using System;
using System.Activities;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using WpfGame.Models;
using WpfGame.Values;
using WpfGame.Views;
using WpfGame.Generals;
using WpfGame.Models.EditModels;
using WpfGame.Tooling;

namespace WpfGame.Controllers.Views
{
    class EditorViewController : ViewController
    {
        private EditorView _editorView;
        private EditorValues _editorValues;
        private const int AmountOfTilesWidth = 20;
        private List<TileEdit> _tileEdits;
        private List<ObstacleEdit> _obstacleEdits;
        private List<CoinEdit> _coinEdits;
        private List<TileObstacleEdit> _tileObstacleEdits;
        private List<TileCoinEdit> _tileCoinEdits;
        private SelectedItem _selectedItem;
        private bool _gotEnd, _gotStart, _gotSpawn;


        public EditorViewController(MainWindow mainWindow) : base(mainWindow)
        {
            _editorView = new EditorView();
            _editorValues = new EditorValues();
            _tileEdits = new List<TileEdit>();
            _obstacleEdits = new List<ObstacleEdit>();
            _coinEdits = new List<CoinEdit>();
            _tileObstacleEdits = new List<TileObstacleEdit>();
            _tileCoinEdits = new List<TileCoinEdit>();

            _editorView.Loaded += EditorCanvas_Loaded;
            _editorView.MouseDown += EditorView_OnMouseDown;

            SetContentOfMain(mainWindow, _editorView);
            SetButtonEvents(_editorView.CancelBtn, CancelBtn_Click);
            SetButtonEvents(_editorView.SaveBtn, SaveBtn_Click);

            _editorView.Focus();

            _gotEnd = _gotSpawn = _gotStart = false;
        }

        private void EditorView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetFocusOnPlaygroundObject();
            UpdatePlaygroundObjects();
        }

        private void UpdatePlaygroundObjects()
        {
            foreach (var tileEdit in _tileEdits)
            {
                if (tileEdit.Rectangle.IsMouseOver)
                {
                    switch (_selectedItem)
                    {
                        case SelectedItem.Wall:
                            tileEdit.IsWall = true;
                            tileEdit.HasCoin = tileEdit.HasObstacle =
                                tileEdit.IsEnd = tileEdit.IsStart = tileEdit.IsSpawn = false;
                            tileEdit.Rectangle.Fill = Brushes.Black;
                            break;
                        case SelectedItem.Obstacle:
                            if (!tileEdit.IsWall && !tileEdit.HasCoin && !tileEdit.IsStart && !tileEdit.IsEnd &&
                                !tileEdit.IsSpawn)
                            {
                                tileEdit.HasObstacle = true;
                                var obs = new ObstacleEdit(tileEdit.Rectangle.Width * .8,
                                    tileEdit.Rectangle.Height * .8, tileEdit.X + (tileEdit.Rectangle.Width * .1),
                                    tileEdit.Y + (tileEdit.Rectangle.Height * .1));
                                _obstacleEdits.Add(obs);
                                Canvas.SetTop(obs.Ellipse, obs.Y);
                                Canvas.SetLeft(obs.Ellipse, obs.X);
                                _editorView.EditorCanvas.Children.Add(obs.Ellipse);
                                _tileObstacleEdits.Add(new TileObstacleEdit(tileEdit, obs));
                            }

                            break;
                        case SelectedItem.Coin:
                            if (!tileEdit.IsWall && !tileEdit.HasObstacle && !tileEdit.IsStart && !tileEdit.IsEnd &&
                                !tileEdit.IsSpawn)
                            {
                                tileEdit.HasCoin = true;
                                var coin = new CoinEdit(tileEdit.Rectangle.Width * .6, tileEdit.Rectangle.Height * .75,
                                    tileEdit.X + (tileEdit.Rectangle.Width * .2),
                                    tileEdit.Y + (tileEdit.Rectangle.Height * .15));
                                _coinEdits.Add(coin);
                                Canvas.SetTop(coin.Ellipse, coin.Y);
                                Canvas.SetLeft(coin.Ellipse, coin.X);
                                _editorView.EditorCanvas.Children.Add(coin.Ellipse);
                                _tileCoinEdits.Add(new TileCoinEdit(tileEdit, coin));
                            }

                            break;
                        case SelectedItem.End:
                            tileEdit.IsEnd = true;
                            tileEdit.HasCoin = tileEdit.HasObstacle =
                                tileEdit.IsWall = tileEdit.IsStart = tileEdit.IsSpawn = false;
                            tileEdit.Rectangle.Fill = Brushes.Red;
                            _gotEnd = true;
                            break;
                        case SelectedItem.Start:
                            tileEdit.IsStart = true;
                            tileEdit.HasCoin = tileEdit.HasObstacle =
                                tileEdit.IsEnd = tileEdit.IsWall = tileEdit.IsSpawn = false;
                            tileEdit.Rectangle.Fill = Brushes.Blue;
                            _gotStart = true;
                            break;
                        case SelectedItem.Spawn:
                            tileEdit.IsSpawn = true;
                            tileEdit.HasCoin = tileEdit.HasObstacle =
                                tileEdit.IsEnd = tileEdit.IsWall = tileEdit.IsStart = false;
                            tileEdit.Rectangle.Fill = Brushes.SaddleBrown;
                            _gotSpawn = true;
                            break;
                        case SelectedItem.Erase:
                            if (tileEdit.IsSpawn)
                            {
                                _gotSpawn = false;
                            }

                            if (tileEdit.IsEnd)
                            {
                                _gotEnd = false;
                            }

                            if (tileEdit.IsStart)
                            {
                                _gotStart = false;
                            }

                            tileEdit.HasCoin = tileEdit.HasObstacle =
                                tileEdit.IsEnd = tileEdit.IsWall = tileEdit.IsStart = tileEdit.IsSpawn = false;
                            tileEdit.Rectangle.Fill = Brushes.Green;
                            break;
                    }
                }
            }

            foreach (var obstacleEdit in _obstacleEdits)
            {
                if (obstacleEdit.Ellipse.IsMouseOver)
                {
                    if (_selectedItem == SelectedItem.Erase)
                    {
                        //ze blijven wel in de array, maar voorzover ik zie boeit dan niet echt, boeit dus wel voor tileEdits, dit moet worden gematched en worden aangepast
                        foreach (var tileObstacleEdit in _tileObstacleEdits)
                        {
                            if (obstacleEdit.Equals(tileObstacleEdit.ObstacleEdit))
                            {
                                _tileEdits.Find(x => x.Equals(tileObstacleEdit.TileEdit)).HasObstacle = false;
                            }
                        }

                        _editorView.EditorCanvas.Children.Remove(obstacleEdit.Ellipse);
                    }
                }
            }

            foreach (var coinEdit in _coinEdits)
            {
                if (coinEdit.Ellipse.IsMouseOver)
                {
                    if (_selectedItem == SelectedItem.Erase)
                    {
                        //ze blijven wel in de array, maar voorzover ik zie boeit dan niet echt, boeit dus wel voor tileEdits, dit moet worden gematched en worden aangepast
                        foreach (var tileCoinEdit in _tileCoinEdits)
                        {
                            if (coinEdit.Equals(tileCoinEdit.CoinEdit))
                            {
                                _tileEdits.Find(x => x.Equals(tileCoinEdit.TileEdit)).HasCoin = false;
                            }
                        }

                        _editorView.EditorCanvas.Children.Remove(coinEdit.Ellipse);
                    }
                }
            }
        }

        private void SetFocusOnPlaygroundObject()
        {
            if (_editorView.WallSelect.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Yellow;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Black;
                _editorView.SpawnSelect.Stroke = Brushes.Black;
                _selectedItem = SelectedItem.Wall;
                _editorView.CurrentSelectedLabel.Content = "Wall";
            }

            if (_editorView.CoinSelect.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Black;
                _editorView.CoinSelect.Stroke = Brushes.Yellow;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Black;
                _editorView.SpawnSelect.Stroke = Brushes.Black;
                _selectedItem = SelectedItem.Coin;
                _editorView.CurrentSelectedLabel.Content = "Coin";
            }

            if (_editorView.ObstSelect.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Black;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Yellow;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Black;
                _editorView.SpawnSelect.Stroke = Brushes.Black;
                _selectedItem = SelectedItem.Obstacle;
                _editorView.CurrentSelectedLabel.Content = "Obstacle";
            }

            if (_editorView.StartSelect.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Black;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Yellow;
                _editorView.EndSelect.Stroke = Brushes.Black;
                _editorView.SpawnSelect.Stroke = Brushes.Black;
                _selectedItem = SelectedItem.Start;
                _editorView.CurrentSelectedLabel.Content = "Startpoint";
            }

            if (_editorView.EndSelect.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Black;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Yellow;
                _editorView.SpawnSelect.Stroke = Brushes.Black;
                _selectedItem = SelectedItem.End;
                _editorView.CurrentSelectedLabel.Content = "Endpoint";
            }

            if (_editorView.SpawnSelect.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Black;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Black;
                _editorView.Erase.Stroke = Brushes.Black;
                _editorView.SpawnSelect.Stroke = Brushes.Yellow;
                _selectedItem = SelectedItem.Spawn;
                _editorView.CurrentSelectedLabel.Content = "SpawnPoint";
            }

            if (_editorView.Erase.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Black;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Black;
                _editorView.Erase.Stroke = Brushes.Yellow;
                _editorView.SpawnSelect.Stroke = Brushes.Black;
                _selectedItem = SelectedItem.Erase;
                _editorView.CurrentSelectedLabel.Content = "Erase";
            }
        }


        private void RenderEditTiles(List<TileEdit> tileEdits)
        {
            foreach (var tileEdit in tileEdits)
            {
                Canvas.SetTop(tileEdit.Rectangle, tileEdit.Y);
                Canvas.SetLeft(tileEdit.Rectangle, tileEdit.X);
                _editorView.EditorCanvas.Children.Add(tileEdit.Rectangle);
            }
        }

        private void LoadEditTiles(List<TileEdit> tileEdits)
        {
            for (int i = 0; i < _editorValues.AmountofYtiles; i++)
            {
                for (int j = 0; j < _editorValues.AmountOfXtiles; j++)
                {
                    tileEdits.Add(new TileEdit(_editorValues.TileWith, _editorValues.TileHeigth,
                        i * _editorValues.TileHeigth, j * _editorValues.TileWith));
                }
            }
        }

        private void SetEditorValues()
        {
            _editorValues.PlayCanvasWidth = _editorValues.OriginMainWindowWidth = MainWindow.ActualWidth;
            _editorValues.OriginMainWindowHeight = MainWindow.ActualHeight;
            _editorView.EditGrid.Height = _editorValues.PlayCanvasHeigth = _editorValues.OriginMainWindowHeight - 30;
            _editorView.ColumnDefinitionOne.Width = new GridLength(_editorValues.PlayCanvasWidth);
            _editorView.ColumnDefinitionTwo.Width = new GridLength(_editorView.EditGrid.Width);
            _editorValues.HeigthWidthRatio = _editorValues.PlayCanvasHeigth / _editorValues.PlayCanvasWidth;
            _editorValues.AmountOfXtiles = AmountOfTilesWidth;
            _editorValues.AmountofYtiles = _editorValues.AmountOfXtiles * _editorValues.HeigthWidthRatio;
            _editorValues.TileWith = _editorValues.PlayCanvasWidth / _editorValues.AmountOfXtiles;
            _editorValues.TileHeigth = _editorValues.PlayCanvasHeigth / _editorValues.AmountofYtiles;

            //we increase the length for the editorpanel and we chop the gamedisplay off
            MainWindow.Width = _editorValues.PlayCanvasWidth + _editorView.EditGrid.Width;
            MainWindow.Height = _editorValues.PlayCanvasHeigth;
        }

        private void SetEditGridVisibility(Visibility visibility)
        {
            _editorView.EditGrid.Visibility = visibility;
        }

        private void EditorCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetEditorValues();
            SetEditGridVisibility(Visibility.Visible);
            LoadEditTiles(_tileEdits);
            RenderEditTiles(_tileEdits);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // we need atleast an spawnpoint and startpoint to create a valid playground. So we do a check on those fields. Endpoint is optional.
            if (!_gotEnd)
            {
                MessageBoxResult continueWithoutEndpoint = MessageBox.Show(
                    "Your playground doesn't seem to have an endpoint, you sure you want to save this playground?",
                    "No Endpoint", MessageBoxButton.YesNo, MessageBoxImage.Question);

                _gotEnd = continueWithoutEndpoint == MessageBoxResult.Yes;
            }

            if (_gotSpawn && _gotStart && _gotEnd)
            {
                try
                {
                    new JsonPlaygroundWriter(_tileEdits);
                    MessageBox.Show("Save succeeded.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBoxResult eraseAfterSave = MessageBox.Show("Erase current playground?", "Erase",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (eraseAfterSave == MessageBoxResult.Yes)
                    {
                        EraseCurrentPlayground();
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show(
                        "We could not create a Playgrounds folder in your root folder. Please create a Playgrounds folder manually and try again.",
                        "Unable to create Playgrounds folder", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (JsonWriterException)
                {
                    MessageBox.Show(
                        "Sorry, something went wrong while creating your playground file. Please try again. ",
                        "Unable to create playground file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (ValidationException)
                {
                    MessageBox.Show("Sorry, your playground is not valid. Please create a new one. ",
                        "Invalid playground",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (IOException)
                {
                    MessageBox.Show(
                        "Unable to write your playground to a file. Please check your disk and user permissions.",
                        "Unable to write playground to file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Something went horribly wrong. Please contact your software-supplier." + ex.Message + " " +
                        ex.StackTrace,
                        "Help", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (!_gotSpawn || !_gotStart || !_gotStart)
            {
                MessageBox.Show(
                    $"Your playground is incomplete. You are missing: {(_gotStart ? "" : "startpoint ") + (_gotEnd ? "" : "endpoint ") + (_gotSpawn ? "" : "spawnpoint")}",
                    "Incomplete playground", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void EraseCurrentPlayground()
        {
            _gotEnd = _gotSpawn = _gotStart = false;
            _tileEdits = new List<TileEdit>();
            LoadEditTiles(_tileEdits);
            RenderEditTiles(_tileEdits);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            //we set the dimension of the screen back to its original
            MainWindow.Width = _editorValues.OriginMainWindowWidth;
            MainWindow.Height = _editorValues.OriginMainWindowHeight;
            new StartWindowViewController(MainWindow);
        }
    }
}