using Android.App;
using Android.Content;
using RemindMe.Android.Services;

namespace RemindMe.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    public class ReminderAlarmReceiver : BroadcastReceiver
    {
        public ReminderAlarmReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (context != null)
            {
                if (context.GetSystemService(Context.NotificationService) is NotificationManager notificationManager)
                {
                    ReminderService.Instance.NotifyReminders(notificationManager, context);
                }
                ReminderService.Instance.StartOrWakeUpService(context);
            }
        }
    }
}