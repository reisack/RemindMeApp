using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using RemindMe.Android.Services;

namespace RemindMe.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true, Permission = "android.permission.RECEIVE_BOOT_COMPLETED")]
    [IntentFilter(new [] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, "android.intent.action.QUICKBOOT_POWERON", "com.htc.intent.action.QUICKBOOT_POWERON" })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent != null && context != null)
                {
                    if (intent.Action == Intent.ActionBootCompleted
                    || intent.Action == Intent.ActionLockedBootCompleted
                    || intent.Action == "android.intent.action.QUICKBOOT_POWERON"
                    || intent.Action == "com.htc.intent.action.QUICKBOOT_POWERON")
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
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
                        else
                        {
                            Intent intentService = new Intent(context, typeof(IntentService));
                            context.StartService(intentService);
                        }
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