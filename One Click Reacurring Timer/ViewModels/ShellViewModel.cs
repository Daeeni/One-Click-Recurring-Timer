using Caliburn.Micro;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace One_Click_Reacurring_Timer.ViewModels
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel()
        {
            
            _timer = TimerBackgroundService.SetTimer(600000);
            _timer.Elapsed += OnTimerEnd;
            AttemptingDeactivation += (sender, args) => _timer.Dispose();
        }

        private System.Timers.Timer _timer;
        private string _time = "23:30";
        private bool _clickedToday = false;
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^(?:[01]?\d|2[0-3])(?::[0-5]\d){1,2}$");
            e.Handled = regex.IsMatch(e.Text);
        }
        public string Time
        {
            get { return _time; }
            set 
            {

                string pattern = @"^(?:[01]?\d|2[0-3])(?::[0-5]\d){1,2}$";
                if (Time != value && Regex.IsMatch(value, pattern))
                {
                    _time = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        public bool ClickedToday
        {
            get { return _clickedToday; }
            set 
            { 
                _clickedToday = value;
                NotifyOfPropertyChange();
            }
        }
        public bool CanClickToday => !_clickedToday;
        public void ClickToday()
        {
            _clickedToday = true;
            NotifyOfPropertyChange(() => ClickedToday);
            NotifyOfPropertyChange(() => CanClickToday);
        }
        private void OnTimerEnd(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (GetMilliseconds() < 600000)
            {
                if (!_clickedToday)
                {
                    SendNotification();
                    ((System.Timers.Timer)source).Interval = 120000;
                }
                else
                {
                    _clickedToday = !_clickedToday;
                    NotifyOfPropertyChange(() => CanClickToday);
                    ((System.Timers.Timer)source).Interval = 600000;
                }
            }

        }
        private int GetMilliseconds()
        {
            DateTime setDateTime = DateTime.ParseExact(Time, "HH:mm", CultureInfo.InvariantCulture);
            DateTime curDateTime = DateTime.Now;
            TimeSpan span = setDateTime - curDateTime;
            if (span.CompareTo(TimeSpan.Zero) < 0)
            {
                span += TimeSpan.FromHours(24);
            }
            return (int)span.TotalMilliseconds;
        }
        private void SendNotification()
        {
            new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
            .AddText("Time is up!")
            .AddText("Notifications will be sent in 2 minute intervals")
            .SetToastScenario(ToastScenario.IncomingCall)
            .Show();
        }
    }
}
