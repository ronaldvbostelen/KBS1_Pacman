using System.Windows;
using System.Windows.Input;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    public class UsernameViewController : ViewController
    {
        private readonly UsernameView _usernameView;
        private readonly string _selectedGameName;

        public UsernameViewController(MainWindow mainWindow, string selectedGameName) : base(mainWindow)
        {
            _usernameView = new UsernameView { tbxUsername = { Text = Settings.Default.Username } };
            _selectedGameName = selectedGameName;
            _usernameView.tbxUsername.Focus();

            SetContentOfMain(mainWindow, _usernameView);
            SetButtonEvents(_usernameView.BtnOk, BtnOk);
            _usernameView.KeyDown += _usernameView_KeyDown;

        }

        private void _usernameView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                new StartWindowViewController(_mainWindow);
            }
        }

        private void BtnOk(object sender, RoutedEventArgs e)
        {
            if(_usernameView.tbxUsername.Text != string.Empty)
            {
                Settings.Default.Username = _usernameView.tbxUsername.Text;
                new GameViewController(_mainWindow, _selectedGameName);
            }
        }
    }
}
