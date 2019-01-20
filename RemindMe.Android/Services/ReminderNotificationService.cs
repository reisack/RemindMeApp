using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Android.Media;
using RemindMe.Core.Models;
using RemindMe.Android.Views;

namespace RemindMe.Android.Services
{
    public class ReminderNotificationService
    {
        public ReminderNotificationService()
        {

        }

        public Notification GetNotification(NotificationManager notificationManager, Reminder reminder, Context context)
        {
            var importance = NotificationImportance.High;
            string channel_id = context.GetString(Resource.String.notification_channel_id);
            string channel_name = context.GetString(Resource.String.notification_channel_name);

            NotificationChannel channel = new NotificationChannel(channel_id, channel_name, importance);
            channel.EnableVibration(true);
            channel.EnableLights(true);
            channel.LockscreenVisibility = NotificationVisibility.Public;

            notificationManager.CreateNotificationChannel(channel);

            // Instantiate the builder and set notification elements
            Notification.Builder builder = new Notification.Builder(context, channel_id);

            builder
                .SetContentTitle(reminder.Title)
                .SetContentText(reminder.Comment)
                .SetContentIntent(GetRemindersListPendingIntent(context))
                .SetAutoCancel(true)
                .SetTicker($"{reminder.Title} {reminder.Comment}")
                .SetSmallIcon(Resource.Drawable.notification_icon);

            // Build the notification
            return builder.Build();
        }

        /// <summary>
        /// Android 7.1 and older compatibility
        /// </summary>
        /// <returns></returns>
        public Notification GetNotificationCompat(NotificationManager notificationManager, Reminder reminder, Context context)
        {
            // Instantiate the builder and set notification elements
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

            builder
                .SetContentTitle(reminder.Title)
                .SetContentText(reminder.Comment)
                .SetContentIntent(GetRemindersListPendingIntent(context))
                .SetAutoCancel(true)
                .SetTicker($"{reminder.Title} {reminder.Comment}")
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));

            // Build the notification
            return builder.Build();
        }

        private PendingIntent GetRemindersListPendingIntent(Context context)
        {
            Intent notificationIntent = new Intent(context, typeof(ReminderListView));
            PendingIntent notificationPendingIntent = PendingIntent.GetActivity(context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return notificationPendingIntent;
        }
    }
}