using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    class StartWindowViewController : ViewController
    {
        private StartWindowView _startWindowView;

        public StartWindowViewController(MainWindow mainWindow) : base(mainWindow)
        {
            _startWindowView = new StartWindowView();

            SetContentOfMain(mainWindow, _startWindowView);
            SetButtonEvents(_startWindowView.btnCloseGame,BtnCloseGame_Click);
            SetButtonEvents(_startWindowView.btnDesignLevel,BtnDesignLevel_Click);
            SetButtonEvents(_startWindowView.btnStartGame,BtnStartGameOnClick);
        }

        private void BtnStartGameOnClick(object sender, RoutedEventArgs e)
        {
            new GameViewController(_mainWindow);
        }

        private void BtnDesignLevel_Click(object sender, RoutedEventArgs e)
        {
            new EditorViewController(_mainWindow);
        }

        private void BtnCloseGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
