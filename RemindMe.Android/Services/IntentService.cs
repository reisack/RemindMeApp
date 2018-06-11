using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using MvvmCross.Platform;
using RemindMe.Android.Services;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;

namespace RemindMe.Android
{
    [Service(Label = "RemindMeIntentService", Enabled = true, Exported = true)]
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

        private async void HandleTimerCallBack(object state)
        {
            try
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
                    if (notificationManager != null && notification != null)
                    {
                        int notificationId = 0;
                        notificationManager.Notify(notificationId, notification);
                    }
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
            catch (Exception ex)
            {
                // TODO : Need to investigate to find what is making app crashes
                // Maybe it's SQLite who is not "ready"
                // For now, the most important thing is to absolutely avoid crashes
            }
        }
    }
}