using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using RemindMe.Core.ViewModels;

namespace RemindMe.Android.Views
{
    [Activity(Label = "Reminders list", MainLauncher = false, Theme = "@style/AppTheme",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReminderListView : MvxAppCompatActivity
    {
        public override View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            Title = GetString(Resource.String.reminders_list);
            return base.OnCreateView(name, context, attrs);
        }

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

            Intent intentService = new Intent(this, typeof(IntentService));

            try
            {
                StartService(intentService);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected override void OnResume()
        {
            var vm = this.ViewModel as ReminderListViewModel;
            if (vm != null)
            {
                vm.OnDeletePastReminders += OnDeletePastReminders;
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
            var toast = Toast.MakeText(this, GetString(Resource.String.past_reminders_deleted_toast), ToastLength.Short);
            toast.Show();
        }
    }
}