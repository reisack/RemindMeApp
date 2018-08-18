using Android.App;
using Android.Content;
using Android.App.Job;
using RemindMe.Android.Services;

namespace RemindMe.Android.Helpers
{
    public static class ReminderJobServiceHelper
    {
        public static void CreateAndScheduleReminderJobService(Context context)
        {
            var javaClass = Java.Lang.Class.FromType(typeof(ReminderJobService));
            var componentName = new ComponentName(context, javaClass);

            JobInfo.Builder builder = new JobInfo.Builder(0, componentName);
            builder.SetPeriodic(60000);
            builder.SetPersisted(true);

            var jobInfo = builder.Build();

            JobScheduler jobScheduler = Application.Context.GetSystemService(Context.JobSchedulerService) as JobScheduler;
            jobScheduler.Schedule(jobInfo);
        }
    }
}