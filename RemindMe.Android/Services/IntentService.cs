using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using MvvmCross.Platform;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;

namespace RemindMe.Android.Services
{
    [Service(Label = "RemindMeIntentService", Enabled = true, Exported = true)]
    public class IntentService : Service
    {
        const int timerCallingDelay = 60000;

        private Timer _timer { get; set; }

        public IntentService()
        {
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            // We want to start the timer on second 0 (callback must be executed at the beginning of each minute)
            var startTime = DateTime.UtcNow;
            var dueTimeInSeconds = (startTime.Second == 0) ? 0 : 60 - startTime.Second;
            _timer = new Timer(HandleTimerCallBack, startTime, ((dueTimeInSeconds + 1) * 1000), timerCallingDelay);

            return StartCommandResult.Sticky;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);

            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        private void HandleTimerCallBack(object state)
        {
            NotificationManager notificationManager = GetSystemService(NotificationService) as NotificationManager;
            ReminderService.Instance.NotifyReminders(notificationManager, this).RunSynchronously();
        }
    }
}