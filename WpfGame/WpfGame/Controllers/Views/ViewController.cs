using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfGame.Controllers.Views
{
    public abstract class ViewController
    {
        protected MainWindow MainWindow;

        protected ViewController(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        protected void SetKeyEvents(UIElement element, KeyEventHandler e)
        {
            element.KeyDown += e;
        }

        protected void SetButtonEvents(Button button, RoutedEventHandler e)
        {
            button.Click += e;
        }

        protected void SetKeyDownEvents(Canvas canvas, KeyEventHandler e)
        {
            canvas.KeyDown += e;
        }

        protected void SetKeyUpEvents(Canvas canvas, KeyEventHandler e)
        {
            canvas.KeyUp += e;
        }
        protected void SetContentOfMain(MainWindow mainWindow, UserControl view)
        {
            mainWindow.Content = view;
        }
    }
}