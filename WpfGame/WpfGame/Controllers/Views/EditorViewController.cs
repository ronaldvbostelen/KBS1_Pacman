using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    class EditorViewController : ViewController
    {
        private EditorView _editorView;

        public EditorViewController(MainWindow mainWindow) : base (mainWindow)
        {
            _editorView = new EditorView();

            SetContentOfMain(mainWindow,_editorView);
            SetButtonEvents(_editorView.ReturnDummy,ReturnDummy_Click);
        }

        private void ReturnDummy_Click(object sender, RoutedEventArgs e)
        {
            new StartWindowViewController(_mainWindow);
        }

    }
}
