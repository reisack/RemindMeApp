using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Platform;
using RemindMe.Android.Helpers;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;

namespace RemindMe.Android.Services
{
    public class ReminderService
    {
        private static Lazy<ReminderService> _instance = new Lazy<ReminderService>(() => new ReminderService());

        public IReminderDataService _reminderDataService;
        public ReminderDaemonDataService _reminderDaemonDataService;

        public ReminderService()
        {
            if (!Mvx.TryResolve<IReminderDataService>(out _reminderDataService))
            {
                _reminderDaemonDataService = new ReminderDaemonDataService();
            }
        }

        public static ReminderService Instance
        {
            get { return _instance.Value; }
        }

        public void WakeUpService(Context context)
        {
            SetAlarmForNextReminder(context);
        }

        public void StartOrWakeUpService(Context context)
        {
            SetAlarmForNextReminder(context);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                ReminderJobServiceHelper.CreateAndScheduleReminderJobService(context);
            }
            else
            {
                Intent intentService = new Intent(context, typeof(Services.IntentService));
                context.StartService(intentService);
            }
        }

        public long? GetMillisTimestampOfNextReminder()
        {
            long? timestamp;
            if (_reminderDataService != null)
            {
                timestamp = _reminderDataService.GetTimestampOfNextReminder();
            }
            else
            {
                timestamp = _reminderDaemonDataService.GetTimestampOfNextReminder();
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
                            notification = ReminderNotificationService.Instance.GetNotification(notificationManager, reminder, context);
                        }
                        else
                        {
                            notification = ReminderNotificationService.Instance.GetNotificationCompat(notificationManager, reminder, context);
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

        private void SetAlarmForNextReminder(Context context)
        {
            long? millisTimestampOfNextReminder = Instance.GetMillisTimestampOfNextReminder();
            if (millisTimestampOfNextReminder.HasValue && millisTimestampOfNextReminder.Value > 0)
            {
                // Alarm is started 10 seconds after notification time
                // So, we have a better guarantee that it will be notified
                long delay = 1000 * 10;

                Intent reminderAlarmReceiver = new Intent(context, Java.Lang.Class.FromType(typeof(ReminderAlarmReceiver)));
                PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, reminderAlarmReceiver, PendingIntentFlags.UpdateCurrent);
                long triggerAtMillis = millisTimestampOfNextReminder.Value + delay;

                AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

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