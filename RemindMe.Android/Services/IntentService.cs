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
using RemindMe.Core.Interfaces;

namespace RemindMe.Android
{
    [Service(Label = "RemindMe Intent Service")]
    public class IntentService : Service
    {
        const int timerCallingDelay = 10000;

        private IReminderDataService _reminderDataService;

        private Timer _timer { get; set; }

        public IntentService()
        {
            _reminderDataService = Mvx.Resolve<IReminderDataService>();
        }


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var startTime = DateTime.UtcNow;
            _timer = new Timer(HandleTimerCallBack, startTime, 0, timerCallingDelay);

            return StartCommandResult.StickyCompatibility;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);
            SendBroadcast(new Intent("REK.RemindMe.Android.RESTART_INTENT_SERVICE"));
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_timer != null)
            {
                _timer.Dispose();
            }
            SendBroadcast(new Intent("REK.RemindMe.Android.RESTART_INTENT_SERVICE"));
        }

        private async void HandleTimerCallBack(object state)
        {
            var reminders = await _reminderDataService.GetRemindersToNotify();

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

            await _reminderDataService.SetToNotified(reminders);
        }
    }
}