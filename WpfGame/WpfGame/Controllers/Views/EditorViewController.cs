using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfGame.Views;

namespace WpfGame.Controllers.Views
{
    class EditorViewController
    {
        private Main _main;
        private EditorView _editorView;

        public EditorViewController(Main main)
        {
            _main = main;
            _editorView = new EditorView();

            SetContentOfMain(_main);
            SetButtonEvents(_editorView);
        }

        private void SetButtonEvents(EditorView view)
        {
            view.ReturnDummy.Click += ReturnDummy_Click;
        }

        private void ReturnDummy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new StartWindowViewController(_main);
        }

        private void SetContentOfMain(Main main)
        {
            main.Content = _editorView;
        }
    }
}
