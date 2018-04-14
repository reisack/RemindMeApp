using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using RemindMe.Android.Services;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;

namespace RemindMe.Android
{
    [Service(Label = "RemindMe Intent Service")]
    public class IntentService : Service
    {
        const int timerCallingDelay = 60000;

        private IReminderDataService _reminderDataService;
        private ReminderDaemonDataService _reminderDaemonDataService;

        private Timer _timer { get; set; }

        public IntentService()
        {
            if (!Mvx.TryResolve<IReminderDataService>(out _reminderDataService))
            {
                _reminderDaemonDataService = new ReminderDaemonDataService();
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var startTime = DateTime.UtcNow;
            var dueTimeInSeconds = (startTime.Second == 0) ? 0 : 60 - startTime.Second;
            _timer = new Timer(HandleTimerCallBack, startTime, (dueTimeInSeconds + 1) * 1000, timerCallingDelay);

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

        private async void HandleTimerCallBack(object state)
        {
            IEnumerable<Reminder> reminders;
            if (_reminderDataService != null)
            {
                reminders = await _reminderDataService.GetRemindersToNotify();
            }
            else
            {
                reminders = await _reminderDaemonDataService.GetRemindersToNotify();
            }

            foreach (var reminder in reminders)
            {
                // Instantiate the builder and set notification elements
                Notification.Builder builder = new Notification.Builder(this);

                builder
                    .SetContentTitle(reminder.Title)
                    .SetContentText(reminder.Comment)
                    .SetSmallIcon(Resource.Drawable.notification_icon)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));

                // Build the notification
                Notification notification = builder.Build();

                // Get the notification manager
                NotificationManager notificationManager = GetSystemService(NotificationService) as NotificationManager;

                // Publish the notification
                int notificationId = 0;
                notificationManager.Notify(notificationId, notification);
            }

            if (_reminderDataService != null)
            {
                await _reminderDataService.SetToNotified(reminders);
            }
            else
            {
                await _reminderDaemonDataService.SetToNotified(reminders);
            }
        }
    }
}