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