using RemindMe.Core.Models;
using System;

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
