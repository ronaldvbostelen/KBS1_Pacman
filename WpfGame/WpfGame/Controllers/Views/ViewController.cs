using System.Windows;
using System.Windows.Controls;

namespace WpfGame.Controllers.Views
{
    public abstract class ViewController
    {
        protected Main _main;

        protected ViewController(Main main)
        {
            _main = main;
        }
        
        protected void SetButtonEvents(Button button, RoutedEventHandler e)
        {
            button.Click += e;
        }
        
        protected void SetContentOfMain(Main main, UserControl view)
        {
            main.Content = view;
        }
    }
}