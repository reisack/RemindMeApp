using Android.App;
using Android.App.Job;
using Android.OS;
using Android.Runtime;

using MvvmCross.Platform;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindMe.Android.Services
{
    [Service(Permission = "android.permission.BIND_JOB_SERVICE")]
    public class ReminderJobService : JobService
    {
        private IReminderDataService _reminderDataService;
        private ReminderDaemonDataService _reminderDaemonDataService;

        public ReminderJobService()
        {
            if (!Mvx.TryResolve<IReminderDataService>(out _reminderDataService))
            {
                _reminderDaemonDataService = new ReminderDaemonDataService();
            }
        }

        protected ReminderJobService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            if (!Mvx.TryResolve<IReminderDataService>(out _reminderDataService))
            {
                _reminderDaemonDataService = new ReminderDaemonDataService();
            }
        }

        public override bool OnStartJob(JobParameters @params)
        {
            // We don't have to await here, we want return true to tell to the system
            // that this task is a background one
            BackgroundWorkAsync(@params);

            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            // Return true results in rescheduling the job based on JobInfo criteria
            return true;
        }

        private async Task BackgroundWorkAsync(JobParameters parameters)
        {
            try
            {
                await Task.Run(PerformBackgroundSyncAsync);
                this.JobFinished(parameters, true);
            }
            catch (Exception ex)
            {
                this.JobFinished(parameters, true);
            }
        }

        private async Task PerformBackgroundSyncAsync()
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