using Caliburn.Micro;
using Microsoft.Toolkit.Uwp.Notifications;
using One_Click_Reacurring_Timer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using One_Click_Reacurring_Timer;

namespace One_Click_Reacurring_Timer.ViewModels
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel()
        {
            List<ClickedModel> listClicked = PersistentStorage.GetZeiten();
            if(listClicked != null )
            {
                if(listClicked.Last<ClickedModel>().Date == DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    _clickedToday = true;
                }
            }
            System.Diagnostics.Debug.WriteLine("test");
            _timer = TimerBackgroundService.SetTimer(360000);
            _timer.Elapsed += OnTimerEnd;
            AttemptingDeactivation += (sender, args) => _timer.Dispose();
        }

        private System.Timers.Timer _timer;
        private string _time = "23:30";
        private bool _clickedToday = false;
        private bool _clickedThisTimerWindow = false;
        private bool _keepTimerAlive = false;
        private int _clickedThisTimerWindowsTTL = 0;

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
            _keepTimerAlive = false;
            ClickedModel toAdd = new ClickedModel(DateTime.Now.ToString("dd.MM.yyyy"), DateTime.Now.ToString("HH:mm"));
            List<ClickedModel> listClicked = PersistentStorage.GetZeiten();
            listClicked.Add(toAdd);
            PersistentStorage.WriteZeitenList(listClicked);
            NotifyOfPropertyChange(() => ClickedToday);
            NotifyOfPropertyChange(() => CanClickToday);
        }
        private void OnTimerEnd(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (_keepTimerAlive)
            {
                SendNotification();
            }
            if (GetMilliseconds() < 600000 && !_clickedThisTimerWindow && !_keepTimerAlive)
            {
                if (!_clickedToday)
                {
                    SendNotification();
                    ((System.Timers.Timer)source).Interval = 120000;
                    _keepTimerAlive = true;
                }
                else
                {
                    _clickedToday = !_clickedToday;
                    _clickedThisTimerWindow = true;
                    _clickedThisTimerWindowsTTL = 3;
                    NotifyOfPropertyChange(() => CanClickToday);
                    ((System.Timers.Timer)source).Interval = 360000;
                }
            }
            if(_clickedThisTimerWindowsTTL == 0)
            {
                _clickedThisTimerWindow = false;
            }
            else
            {
                _clickedThisTimerWindowsTTL--;
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
