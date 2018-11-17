using System;
using Android.App;
using Android.Content;
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
                        ReminderService.Instance.StartOrWakeUpService(context);
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