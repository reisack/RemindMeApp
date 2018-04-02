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

namespace RemindMe.Android
{
    [Service]
    public class IntentService : Service
    {
        const int timerCallingDelay = 10000;

        private Timer _timer { get; set; }


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var startTime = DateTime.UtcNow;
            _timer = new Timer(HandleTimerCallBack, startTime, 0, timerCallingDelay);

            return StartCommandResult.Sticky;
        }

        private void HandleTimerCallBack(object state)
        {
            // Instantiate the builder and set notification elements
            Notification.Builder builder = new Notification.Builder(this);

            builder
                .SetContentTitle("Mon titre - via service")
                .SetContentText("Hello world ! Via service !")
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
    }
}