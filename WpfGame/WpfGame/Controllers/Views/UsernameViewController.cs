using System.Windows;
using WpfGame.Properties;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class UsernameViewController : ViewController
    {
        private readonly UsernameView _usernameView;
        private readonly string _selectedGameName;

        public UsernameViewController(MainWindow mainWindow, string selectedGameName) : base(mainWindow)
        {
            _usernameView = new UsernameView { tbxUsername = {Text = Settings.Default.Username}};
            _selectedGameName = selectedGameName;

            SetContentOfMain(mainWindow, _usernameView);
            SetButtonEvents(_usernameView.BtnOk, BtnOk);
        }

        private void BtnOk(object sender, RoutedEventArgs e)
        {
            Settings.Default.Username = _usernameView.tbxUsername.Text;
            new GameViewController(_mainWindow, _selectedGameName);
        }
    }
}
