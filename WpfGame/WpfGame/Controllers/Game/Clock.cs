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

namespace WpfGame.Controllers
{
    class Clock
    {
        private DispatcherTimer timer;
        private TextBlock tblTimer;
        private TimeSpan _time;

        public Clock()
        {
            InitializeTimer();
            SetTimerDisplayProperties();

            ControlRenderer.Draw(new Position(800, 30), tblTimer);
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();

            _time = TimeSpan.FromSeconds(10); // Count down from 60 seconds

            // Call this every 1 second
            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                tblTimer.Text = _time.ToString("mm':'ss"); // Display the time in the timers textblock with this format: "00:00"
                if (_time == TimeSpan.Zero) // Execute when the timer has reached zero
                {
                    Timer_Elapsed();
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1)); // Remove one second from the timers timespan
            }, Application.Current.Dispatcher);

            timer.Start();
        }

        private void SetTimerDisplayProperties()
        {
            tblTimer = new TextBlock();
            tblTimer.Height = 50;
            tblTimer.Width = 200;
            tblTimer.Visibility = Visibility.Visible;
            tblTimer.FontSize = 20;
            tblTimer.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Timer_Elapsed()
        {
            Console.WriteLine("Timer is gestopt");
            timer.Stop();
        }
    }
}
