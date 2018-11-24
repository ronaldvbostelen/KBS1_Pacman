using System.Windows;
using System.Windows.Controls;

namespace WpfGame.Controllers.Views
{
    public abstract class ViewController
    {
        protected MainWindow _mainWindow;

        protected ViewController(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        
        protected void SetButtonEvents(Button button, RoutedEventHandler e)
        {
            button.Click += e;
        }
        
        protected void SetContentOfMain(MainWindow mainWindow, UserControl view)
        {
            mainWindow.Content = view;
        }
    }
}