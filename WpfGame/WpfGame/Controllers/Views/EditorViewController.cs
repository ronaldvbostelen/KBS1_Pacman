using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfGame.Editor;
using WpfGame.Models;
using WpfGame.Values;
using WpfGame.Views;
using WpfGame.Generals;

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
        

        public EditorViewController(MainWindow mainWindow) : base (mainWindow)
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
        }

        private void EditorView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_editorView.WallSelect.IsMouseOver)
            {
                _editorView.WallSelect.Stroke = Brushes.Yellow;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Black;
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
                _selectedItem = SelectedItem.End;
                _editorView.CurrentSelectedLabel.Content = "Endpoint";
            }

            if (_editorView.Erase.IsMouseOver)
            {

                _editorView.WallSelect.Stroke = Brushes.Black;
                _editorView.CoinSelect.Stroke = Brushes.Black;
                _editorView.ObstSelect.Stroke = Brushes.Black;
                _editorView.StartSelect.Stroke = Brushes.Black;
                _editorView.EndSelect.Stroke = Brushes.Black;
                _editorView.Erase.Stroke = Brushes.Yellow;
                _selectedItem = SelectedItem.Erase;
                _editorView.CurrentSelectedLabel.Content = "Erase";
            }

            foreach (var tileEdit in _tileEdits)
            {
                if (tileEdit.Rectangle.IsMouseOver)
                {
                    switch (_selectedItem)
                    {
                        case SelectedItem.Wall:
                            if (!tileEdit.HasObstacle && !tileEdit.HasCoin)
                            { 
                                tileEdit.IsWall = true;
                                tileEdit.Rectangle.Fill = Brushes.Black;
                            }
                            break;
                        case SelectedItem.Obstacle:
                            if (!tileEdit.IsWall && !tileEdit.HasCoin)
                            {
                                tileEdit.HasObstacle = true;
                                var obs = new ObstacleEdit(tileEdit.Rectangle.Width*.8,tileEdit.Rectangle.Height*.8,tileEdit.X + (tileEdit.Rectangle.Width*.1) ,
                                    tileEdit.Y +(tileEdit.Rectangle.Height * .1) );
                                _obstacleEdits.Add(obs);
                                Canvas.SetTop(obs.Ellipse,obs.Y);
                                Canvas.SetLeft(obs.Ellipse,obs.X);
                                _editorView.EditorCanvas.Children.Add(obs.Ellipse);
                                _tileObstacleEdits.Add(new TileObstacleEdit(tileEdit,obs));
                            }
                            break;
                        case SelectedItem.Coin:
                            if (!tileEdit.IsWall && !tileEdit.HasObstacle)
                            {
                                tileEdit.HasCoin = true;
                                var coin = new CoinEdit(tileEdit.Rectangle.Width * .6, tileEdit.Rectangle.Height * .75, tileEdit.X + (tileEdit.Rectangle.Width * .2),
                                    tileEdit.Y + (tileEdit.Rectangle.Height * .15));
                                _coinEdits.Add(coin);
                                Canvas.SetTop(coin.Ellipse, coin.Y);
                                Canvas.SetLeft(coin.Ellipse, coin.X);
                                _editorView.EditorCanvas.Children.Add(coin.Ellipse);
                                _tileCoinEdits.Add(new TileCoinEdit(tileEdit,coin));
                            }
                            break;
                        case SelectedItem.End:
                            if (!tileEdit.IsWall)
                            {
                                tileEdit.IsEnd = true;
                            }

                            //endvisual
                            break;
                        case SelectedItem.Start:
                            if (!tileEdit.IsWall)
                            {
                                tileEdit.IsEnd = true;
                            }
                            //endvisual
                            break;
                        case SelectedItem.Erase:
                            tileEdit.HasCoin = tileEdit.HasObstacle =
                                tileEdit.IsEnd = tileEdit.IsWall = tileEdit.IsStart = false;
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


        private void RenderEditTiles(List<TileEdit> tileEdits)
        {
            foreach (var tileEdit in tileEdits)
            {
                Canvas.SetTop(tileEdit.Rectangle,tileEdit.Y);
                Canvas.SetLeft(tileEdit.Rectangle,tileEdit.X);
                _editorView.EditorCanvas.Children.Add(tileEdit.Rectangle);
            }
        }

        private void LoadEditTiles(List<TileEdit> tileEdits)
        {
            for (int i = 0; i < _editorValues.AmountofYtiles; i++)
            {
                for (int j = 0; j < _editorValues.AmountOfXtiles; j++)
                {
                    tileEdits.Add(new TileEdit(_editorValues.TileWith,_editorValues.TileHeigth,i*_editorValues.TileHeigth,j*_editorValues.TileWith, false, false, false, false, false));
                }
            }
        }

        private void SetEditorValues()
        {
            _editorValues.PlayCanvasWidth = _editorValues.OriginMainWindowWidth = _mainWindow.ActualWidth;
            _editorValues.PlayCanvasHeigth = _editorView.EditorCanvas.ActualHeight;
            _editorView.ColumnDefinitionOne.Width = new GridLength(_editorValues.PlayCanvasWidth);
            _editorView.ColumnDefinitionTwo.Width = new GridLength(_editorView.EditGrid.Width);
            _mainWindow.Width = _editorValues.PlayCanvasWidth + _editorView.EditGrid.Width;
            _editorValues.HeigthWidthRatio = _editorValues.PlayCanvasHeigth / _editorValues.PlayCanvasWidth;
            _editorValues.AmountOfXtiles = AmountOfTilesWidth;
            _editorValues.AmountofYtiles = _editorValues.AmountOfXtiles * _editorValues.HeigthWidthRatio;
            _editorValues.TileWith = _editorValues.PlayCanvasWidth / _editorValues.AmountOfXtiles;
            _editorValues.TileHeigth = _editorValues.PlayCanvasHeigth / _editorValues.AmountofYtiles;
        }

        private void SetEditGridVisibility(Visibility visibility)
        {
            _editorView.EditGrid.Visibility = visibility;
        }

        private void EditorCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetEditorValues();
            SetEditGridVisibility(Visibility.Visible);

            //dit is niet heel charmant, maar anders zijn de waardes niet geladen voor de aanroep, als iemand het beter weet: doen (en mij vertellen :))
            LoadEditTiles(_tileEdits);
            RenderEditTiles(_tileEdits);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            new JsonPlaygroundWriter(_tileEdits);
            MessageBox.Show("Save geslaagd", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Width = _editorValues.OriginMainWindowWidth;
            new StartWindowViewController(_mainWindow);
        }
    }
}
