using System;
using Android.App;
using Android.Content;
using Android.Support.V4.Content;
using MvvmCross.Platform;
using RemindMe.Android.Helpers;
using RemindMe.Android.Services;
using RemindMe.Core.Interfaces;

namespace RemindMe.Android
{
    public class ReminderAlarmReceiver : WakefulBroadcastReceiver
    {
        private IReminderDataService _reminderDataService;
        private ReminderDaemonDataService _reminderDaemonDataService;

        public ReminderAlarmReceiver()
        {
            if (!Mvx.TryResolve<IReminderDataService>(out _reminderDataService))
            {
                _reminderDaemonDataService = new ReminderDaemonDataService();
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            ReminderService.Instance.NotifyReminders(notificationManager, context).RunSynchronously();
            StartWakefulService(context, intent);
        }
    }
}