using System;
using System.Threading.Tasks;

using Android.App;
using Android.App.Job;
using Android.Runtime;

namespace RemindMe.Android.Services
{
    [Service(Permission = "android.permission.BIND_JOB_SERVICE")]
    public class ReminderJobService : JobService
    {
        public ReminderJobService()
        {
        }

        protected ReminderJobService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool OnStartJob(JobParameters @params)
        {
            // We don't have to await here, we want to return true
            // as soon as possible to tell to the system
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
            if (GetSystemService(NotificationService) is NotificationManager notificationManager)
            {
                await ReminderService.Instance.NotifyReminders(notificationManager, this);
            }
        }
    }
}