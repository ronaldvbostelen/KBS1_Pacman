using System.Windows;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class UsernameViewController : ViewController
    {
        private readonly UsernameView _usernameView;

        public UsernameViewController(MainWindow mainWindow) : base(mainWindow)
        {
            _usernameView = new UsernameView {username = {Text = Settings.Default.Username}};

            SetContentOfMain(mainWindow, _usernameView);
            SetButtonEvents(_usernameView.BtnOk, BtnOk);
            SetButtonEvents(_usernameView.BtnCancel, BtnCancel);
        }

        private void BtnOk(object sender, RoutedEventArgs e)
        {
            Settings.Default.Username = _usernameView.username.Text;
            new StartWindowViewController(_mainWindow);
        }

        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            new StartWindowViewController(_mainWindow);
        }
    }
}
