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
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // https://forums.xamarin.com/discussion/5198/how-do-i-create-an-options-menu
            // https://developer.android.com/guide/topics/ui/menus.html
            MenuInflater.Inflate(Resource.Layout.menu_list, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.remove_past:
                    // TODO
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

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