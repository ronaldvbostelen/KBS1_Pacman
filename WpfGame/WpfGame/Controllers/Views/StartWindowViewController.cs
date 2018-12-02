using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfGame.Generals;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    class StartWindowViewController : ViewController
    {
        private StartWindowView _startWindowView;
        private bool selectedPlayground;

        public StartWindowViewController(MainWindow mainWindow) : base(mainWindow)
        {
            _startWindowView = new StartWindowView();
            selectedPlayground = false;

            SetContentOfMain(mainWindow, _startWindowView);

            SetButtonEvents(_startWindowView.btnCloseGame, BtnCloseGame_Click);
            SetButtonEvents(_startWindowView.btnDesignLevel, BtnDesignLevel_Click);
            SetButtonEvents(_startWindowView.btnStartGame, BtnStartGameOnClick);
            SetButtonEvents(_startWindowView.CancelSelectPlgrnd, BtnCancelSelect_Click);
            SetButtonEvents(_startWindowView.SelectPlgrnd, BtnConfirmSelect_Click);
            SetButtonEvents(_startWindowView.BtnHighScoreTable, BtnHighScoreTable_Click);

            SetKeyEvents(_startWindowView.Grid, Grid_KeyDown);

            //todo:wrapper
            _startWindowView.Grid.Loaded += Grid_Loaded;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            _startWindowView.Grid.Focus();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    FileInfo[] files =
                        new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + General.playgroundPath)
                            .GetFiles();
                    _startWindowView.ListBoxForPlaygroundFiles.ItemsSource = files;
                    _startWindowView.SelectPlaygroundMenu.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void BtnCancelSelect_Click(object sender, RoutedEventArgs e)
        {
            _startWindowView.SelectPlaygroundMenu.Visibility = Visibility.Collapsed;
        }

        private void BtnConfirmSelect_Click(object sender, RoutedEventArgs e)
        {
            _startWindowView.SelectPlaygroundMenu.Visibility = Visibility.Collapsed;
            MessageBox.Show("Level selected, you can start playing", "Well done", MessageBoxButton.OK,
                MessageBoxImage.Information);
            Settings.Default.Level = _startWindowView.ListBoxForPlaygroundFiles.SelectedItem.ToString();
        }

        private void BtnStartGameOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.Level))
            {
                //if non is selected we select the newest playground
                var directory = new DirectoryInfo($"{Environment.CurrentDirectory}\\Playgrounds");
                Settings.Default.Level =
                        (from f in directory.GetFiles()
                        orderby f.LastWriteTime descending
                        select f).First().ToString();
            }
            
            //Call the UsernameViewController when the Username has not been set
            if (Settings.Default.Username == string.Empty) 
            {
                new UsernameViewController(_mainWindow, Settings.Default.Level);
            }
            
            new GameViewController(_mainWindow, Settings.Default.Level);
        }

        private void BtnDesignLevel_Click(object sender, RoutedEventArgs e)
        {
            new EditorViewController(_mainWindow);
        }

        private void BtnCloseGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnHighScoreTable_Click (object sender, RoutedEventArgs e)
        {
            new HighScoreViewController(_mainWindow);
        }
    }
}
