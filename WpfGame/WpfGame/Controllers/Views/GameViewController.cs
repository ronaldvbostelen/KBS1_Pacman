using System.Windows;
using System.Windows.Controls;
using WpfGame;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class GameViewController : ViewController
    {
        private GameView _gameView;
        public static Canvas Canvas;

        public GameViewController(MainWindow mainWindow) 
            : base(mainWindow) 
        {
            _gameView = new GameView();
            Canvas = _gameView.GameCanvas;
            Player player = new Player();
            Clock clock = new Clock();

            SetContentOfMain(mainWindow, _gameView);
            SetButtonEvents(_gameView.ReturnDummy, ReturnDummy_Click);
        }

        private void ReturnDummy_Click(object sender, RoutedEventArgs e)
        {
            new StartWindowViewController(_mainWindow);
        }

    }
}