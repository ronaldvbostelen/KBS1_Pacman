using WpfGame;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class GameViewController
    {
        private Main _main;
        private GameView _gameView;

        public GameViewController(Main main)
        {
            _main = main;
            _gameView = new GameView();


            SetContentOfMain(_main);
            SetButtonEvents(_gameView);
        }

        private void SetButtonEvents(GameView view)
        {
            view.ReturnDummy.Click += ReturnDummy_Click;
        }

        private void ReturnDummy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new StartWindowViewController(_main);
        }

        private void SetContentOfMain(Main main)
        {
            main.Content = _gameView;
        }
    }
}