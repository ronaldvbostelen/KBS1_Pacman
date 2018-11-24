using System.Windows;
using WpfGame;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class GameViewController : ViewController
    {
        private GameView _gameView;

        public GameViewController(MainWindow mainWindow) : base(mainWindow) 
        {
            _gameView = new GameView();

            SetContentOfMain(mainWindow,_gameView);
            SetButtonEvents(_gameView.ReturnDummy,ReturnDummy_Click);
        }


        private void ReturnDummy_Click(object sender, RoutedEventArgs e)
        {
            new StartWindowViewController(_mainWindow);
        }

    }
}