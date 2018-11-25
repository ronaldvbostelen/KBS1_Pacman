using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using WpfGame.Controllers.Behaviour;
using WpfGame.Controllers.Renderer;
using WpfGame.Controllers.Views;

namespace WpfGame.Controllers
{
    class ClockController : ViewController
    {
        private DispatcherTimer _timer;
        private TextBlock _tblTimer;
        private TimeSpan _time;

        public ClockController(MainWindow mainWindow) 
            : base(mainWindow)
        {
            InitializeTimer();
            SetTimerDisplayProperties();

            ControlRenderer.Draw(new Position(800, 30), _tblTimer);
        }

        /**
         * Initializes a new timer with a delegate event
         * which executes every second in order to display
         * a realtime clock for the player
         **/
        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();

            _time = TimeSpan.FromSeconds(60); // Count down from 60 seconds

            // Call this every 1 second
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                _tblTimer.Text = _time.ToString("mm':'ss"); // Display the time in the timers textblock with this format: "00:00"
                if (_time == TimeSpan.Zero) // Execute when the timer has reached zero
                {
                    Timer_Elapsed();
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1)); // Remove one second from the timers timespan
            }, Application.Current.Dispatcher);

            _timer.Start();
        }

        /**
         *  This sets the properties of the textblock in which
         *  the countdown of the timer will be displayed
         **/
        private void SetTimerDisplayProperties()
        {
            _tblTimer = new TextBlock();
            _tblTimer.Height = 50;
            _tblTimer.Width = 200;
            _tblTimer.Visibility = Visibility.Visible;
            _tblTimer.FontSize = 20;
            _tblTimer.Foreground = new SolidColorBrush(Colors.White);
        }

        /**
         *  Every action that should be executed when the timer
         *  has reached zero
         **/
        private void Timer_Elapsed()
        {
            _timer.Stop();
            //ToDo: fill in the total score
            MessageBox.Show("Time's up! Total score: ***", "", MessageBoxButton.OK);

            new StartWindowViewController(_mainWindow); // Return to start window
        }
    }
}
