﻿using System;
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
                    ReminderNotificationService reminderNotificationService = new ReminderNotificationService();

                    // Get the notification manager
                    NotificationManager notificationManager = GetSystemService(NotificationService) as NotificationManager;
                    Notification notification;
                    int notificationId = 0;

                    foreach (var reminder in reminders)
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                        {
                            notification = reminderNotificationService.GetNotification(notificationManager, reminder, this);
                        }
                        else
                        {
                            notification = reminderNotificationService.GetNotificationCompat(notificationManager, reminder, this);
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
    }
}