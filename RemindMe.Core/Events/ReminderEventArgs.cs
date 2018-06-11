using RemindMe.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemindMe.Core.Events
{
    public class ReminderEventArgs : EventArgs
    {
        public ReminderEventArgs(Reminder reminder)
        {
            Reminder = reminder;
        }

        public Reminder Reminder { get; }
    }
}
