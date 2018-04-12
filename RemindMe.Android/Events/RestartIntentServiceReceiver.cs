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

namespace RemindMe.Android.Events
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new [] { "REK.RemindMe.Android.RESTART_INTENT_SERVICE" })]
    public class RestartIntentServiceReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (!ServicesHelper.IsServiceRunning(context, typeof(IntentService)))
            {
                Intent intentService = new Intent(context, typeof(IntentService));
                context.StartService(intentService);
            }
        }
    }
}