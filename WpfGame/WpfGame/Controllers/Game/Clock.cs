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
using WpfGame.Generals;

namespace WpfGame.Controllers
{
    class Clock 
    {   
        public string Display { get; set; }
        private DispatcherTimer _timer;
        private TimeSpan _time;
        public event PlaytimeIsOVerEventHandeler PlaytimeIsOVerEventHander;
        
        /**
         * Initializes a new timer with a delegate event
         * which executes every second in order to display
         * a realtime clock for the player
         **/
        public void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _time = TimeSpan.FromSeconds(60); // Count down from 60 seconds
            

            // Call this every second
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                Display = _time.ToString("mm':'ss"); // Display the time in the timers textblock with this format: "00:00"

                if (_time == TimeSpan.Zero) // Execute when the timer has reached zero
                {
                    Timer_Elapsed();
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1)); // Remove one second from the timers timespan

            }, Application.Current.Dispatcher);

            _timer.Start();
        }

        /**
         *  Every action that should be executed when the timer
         *  has reached zero
         **/
        private void Timer_Elapsed()
        {
            _timer.Stop();
            PlaytimeIsOver();
        }

        protected virtual void PlaytimeIsOver()
        {
            PlaytimeIsOVerEventHander?.Invoke(this,EventArgs.Empty);
        }
    }
}
