using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Platform;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;

namespace RemindMe.Android.Services
{
    public class ReminderService
    {
        private static Lazy<ReminderService> _singletonInstance = new Lazy<ReminderService>(() => new ReminderService());
        private ReminderNotificationService _reminderNotificationService;

        public IReminderDataService _reminderDataService;
        public ReminderDaemonDataService _reminderDaemonDataService;

        public ReminderService()
        {
            if (!Mvx.TryResolve<IReminderDataService>(out _reminderDataService))
            {
                _reminderDaemonDataService = new ReminderDaemonDataService();
            }

            _reminderNotificationService = new ReminderNotificationService();
        }

        public static ReminderService SingletonInstance
        {
            get { return _singletonInstance.Value; }
        }

        public long? GetNextReminderMillisTimestamp()
        {
            long? timestamp;
            if (_reminderDataService != null)
            {
                timestamp = _reminderDataService.GetNextReminderTimestamp();
            }
            else
            {
                timestamp = _reminderDaemonDataService.GetNextReminderTimestamp();
            }
            return (timestamp.HasValue) ? timestamp * 1000 : null;
        }

        public async Task NotifyReminders(NotificationManager notificationManager, Context context)
        {
            try
            {
                IList<Reminder> reminders;
                if (_reminderDataService != null)
                {
                    reminders = new List<Reminder>(await _reminderDataService.GetRemindersToNotify());
                }
                else
                {
                    reminders = new List<Reminder>(await _reminderDaemonDataService.GetRemindersToNotify());
                }

                if (reminders.Count > 0)
                {
                    Notification notification;
                    int notificationId = 0;

                    foreach (var reminder in reminders)
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                        {
                            notification = _reminderNotificationService.GetNotification(notificationManager, reminder, context);
                        }
                        else
                        {
                            notification = _reminderNotificationService.GetNotificationCompat(notificationManager, reminder, context);
                        }

                        // Publish the notification
                        if (notificationManager != null && notification != null)
                        {
                            notificationManager.Notify(notificationId, notification);
                            notificationId++;
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void SetAlarmForNextReminder(Context context)
        {
            long? nextReminderMillisTimestamp = GetNextReminderMillisTimestamp();
            if (nextReminderMillisTimestamp.HasValue && nextReminderMillisTimestamp.Value > 0)
            {
                // Alarm is started 5 seconds after notification time
                // So, we have a better guarantee that it will be notified
                long delay = 1000 * 5;

                Intent reminderAlarmReceiver = new Intent(context, Java.Lang.Class.FromType(typeof(ReminderAlarmReceiver)));
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, reminderAlarmReceiver, PendingIntentFlags.UpdateCurrent);
                long triggerAtMillis = nextReminderMillisTimestamp.Value + delay;

                AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;

                if (alarmManager != null)
                {
                    // Method SetExactAndAllowWhileIdle() is for Doze mode, introduced in Android 6.0
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
                    }
                    else
                    {
                        alarmManager.SetExact(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
                    }
                }
            }
        }
    }
}