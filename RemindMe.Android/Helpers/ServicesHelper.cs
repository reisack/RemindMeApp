using System;
using Android.App;
using Android.Content;

namespace RemindMe.Android.Helpers
{
    public static class ServicesHelper
    {
        public static bool IsServiceRunning(Context context, Type serviceClass)
        {
            ActivityManager activityManager = context.GetSystemService(Context.ActivityService) as ActivityManager;
            if (activityManager != null)
            {
                foreach (var service in activityManager.GetRunningServices(int.MaxValue))
                {
                    if (service.Process == context.PackageName
                        && service.Service.ClassName.EndsWith(serviceClass.Name))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}