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
using MvvmCross.Droid.Views;
using RemindMe.Android.Helpers;
using RemindMe.Core.ViewModels;

namespace RemindMe.Android.Views
{
    [Activity(Label = "Reminders list", MainLauncher = true)]
    public class ReminderListView : MvxActivity
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.ReminderList);

            if (!ServicesHelper.IsServiceRunning(this, typeof(IntentService)))
            {
                Intent intentService = new Intent(this, typeof(IntentService));
                StartService(intentService);
            }
        }
    }
}