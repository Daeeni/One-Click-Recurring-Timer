using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace One_Click_Reacurring_Timer
{
    public class TimerBackgroundService
    {
        public static System.Timers.Timer SetTimer(int ms)
        {
            System.Timers.Timer timer = new System.Timers.Timer(ms);
            timer.AutoReset = true;
            timer.Enabled = true;
            return timer;
        }
    }
}
