using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RemindMe.Android.Services
{
    public class ReminderNotificationService
    {
        public ReminderNotificationService()
        {

        }

        public Notification GetNotification(NotificationManager notificationManager, Reminder reminder)
        {
            var importance = NotificationImportance.High;
            string channel_id = GetString(Resource.String.notification_channel_id);
            string channel_name = GetString(Resource.String.notification_channel_name);

            NotificationChannel channel = new NotificationChannel(channel_id, channel_name, importance);
            channel.EnableVibration(true);
            channel.LockscreenVisibility = NotificationVisibility.Public;

            notificationManager.CreateNotificationChannel(channel);

            // Instantiate the builder and set notification elements
            Notification.Builder builder = new Notification.Builder(this);

            builder
                .SetContentTitle(reminder.Title)
                .SetContentText(reminder.Comment)
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetChannelId(channel_id);

            // Build the notification
            return builder.Build();
        }

        /// <summary>
        /// Android 7.1 and older compatibility
        /// </summary>
        /// <returns></returns>
        public Notification GetNotificationCompat(NotificationManager notificationManager, Reminder reminder)
        {
            // Instantiate the builder and set notification elements
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

            builder
                .SetContentTitle(reminder.Title)
                .SetContentText(reminder.Comment)
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));

            // Build the notification
            return builder.Build();
        }
    }
}