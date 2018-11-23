using WpfGame;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class GameViewController : ViewController
    {
        private GameView _gameView;

        public GameViewController(Main main) : base(main) 
        {
            _gameView = new GameView();

            SetContentOfMain(main,_gameView);
            SetButtonEvents(_gameView.ReturnDummy,ReturnDummy_Click);
        }


        private void ReturnDummy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new StartWindowViewController(_main);
        }

    }
}