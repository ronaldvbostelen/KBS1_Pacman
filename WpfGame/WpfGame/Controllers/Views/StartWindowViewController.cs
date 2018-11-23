using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    class StartWindowViewController
    {
        private StartWindowView _startWindowView;
        private Main _main;

        public StartWindowViewController(Main main)
        {
            _main = main;
            _startWindowView = new StartWindowView();

            SetContentOfMain(_main);
            SetButtonEvents(_startWindowView);
        }

        private void SetContentOfMain(Main main)
        {
            main.Content = _startWindowView;
        }

        private void SetButtonEvents(StartWindowView view)
        {
            view.btnCloseGame.Click += BtnCloseGame_Click;
            view.btnDesignLevel.Click += BtnDesignLevel_Click;
            view.btnStartGame.Click += BtnStartGameOnClick;
        }

        private void BtnStartGameOnClick(object sender, RoutedEventArgs e)
        {
            new GameViewController(_main);
        }

        private void BtnDesignLevel_Click(object sender, RoutedEventArgs e)
        {
            new EditorViewController(_main);
        }

        private void BtnCloseGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
