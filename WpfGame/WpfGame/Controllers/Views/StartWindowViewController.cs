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

        public StartWindowViewController(Main main) : base(main)
        {
            _startWindowView = new StartWindowView();

            SetContentOfMain(main, _startWindowView);
            SetButtonEvents(_startWindowView.btnCloseGame,BtnCloseGame_Click);
            SetButtonEvents(_startWindowView.btnDesignLevel,BtnDesignLevel_Click);
            SetButtonEvents(_startWindowView.btnStartGame,BtnStartGameOnClick);
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
