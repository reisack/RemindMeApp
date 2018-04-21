using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using RemindMe.Android.Helpers;
using RemindMe.Core.Interfaces;
using RemindMe.Core.ViewModels;

namespace RemindMe.Android.Views
{
    [Activity(Label = "Reminders list", MainLauncher = false, Theme = "@style/AppTheme",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReminderListView : MvxAppCompatActivity
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
                    var vm = this.ViewModel as ReminderListViewModel;
                    if (vm != null)
                    {
                        vm.DeletePastRemindersCommand.Execute();
                    }
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

        protected override void OnResume()
        {
            var vm = this.ViewModel as ReminderListViewModel;
            if (vm != null)
            {
                vm.OnDeletePastReminders += OnDeletePastReminders; ;
            }

            base.OnResume();
        }

        protected override void OnPause()
        {
            var vm = this.ViewModel as ReminderListViewModel;
            if (vm != null)
            {
                vm.OnDeletePastReminders -= OnDeletePastReminders;
            }

            base.OnPause();
        }

        private void OnDeletePastReminders(object sender, EventArgs e)
        {
            var toast = Toast.MakeText(this, "Past reminders succesfully deleted", ToastLength.Short);
            toast.Show();
        }
    }
}