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
using RemindMe.Android.Helpers;

namespace RemindMe.Android
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new [] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, "android.intent.action.QUICKBOOT_POWERON", "com.htc.intent.action.QUICKBOOT_POWERON" })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionBootCompleted 
                || intent.Action == Intent.ActionLockedBootCompleted
                || intent.Action == "android.intent.action.QUICKBOOT_POWERON"
                || intent.Action == "com.htc.intent.action.QUICKBOOT_POWERON")
            {
                if (!ServicesHelper.IsServiceRunning(context, typeof(IntentService)))
                {
                    Intent intentService = new Intent(context, typeof(IntentService));
                    context.StartService(intentService);
                }
            }
        }
    }
}