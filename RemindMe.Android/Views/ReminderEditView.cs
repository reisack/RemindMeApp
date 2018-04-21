using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using RemindMe.Core.Converters;
using RemindMe.Core.ViewModels;
using RemindMe.Android.Extensions;

namespace RemindMe.Android.Views
{
    [Activity(Label = "Edit", MainLauncher = false, Theme = "@style/AppTheme",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReminderEditView : MvxAppCompatActivity, DatePickerDialog.IOnDateSetListener, TimePickerDialog.IOnTimeSetListener
    {
        private EditText _reminderDatePickerText;
        private EditText _reminderDatePickerEditTextReadableValue;
        private EditText _reminderTimePickerText;
        private EditText _reminderTimePickerEditTextReadableValue;

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            var vm = this.ViewModel as ReminderEditViewModel;
            if (vm != null)
            {
                if (vm.IsUpdateMode)
                {
                    Title = "Update reminder";
                }
                else
                {
                    Title = "Create reminder";
                }
            }
            
            return base.OnCreateView(parent, name, context, attrs);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // https://forums.xamarin.com/discussion/5198/how-do-i-create-an-options-menu
            // https://developer.android.com/guide/topics/ui/menus.html
            MenuInflater.Inflate(Resource.Layout.menu_edit, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.edit_back:
                    OnBackPressed();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.ReminderEdit);

            SetDatePickerText();
            SetTimePickerText();

            // Refresh view model
            var vvmBinding = this.CreateBindingSet<ReminderEditView, ReminderEditViewModel>();
            vvmBinding.Bind(_reminderDatePickerText).To(vm => vm.ReminderDay);
            vvmBinding.Bind(_reminderTimePickerText).To(vm => vm.ReminderTime);
            vvmBinding.Apply();
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            DateTime selectedDate = new DateTime(year, month + 1, dayOfMonth);
            _reminderDatePickerText.Text = selectedDate.ToString();
            _reminderDatePickerEditTextReadableValue.Text = ReadableDateConverter.Convert(selectedDate);
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            _reminderTimePickerText.Text = string.Format("{0}:{1}", hourOfDay.ToString("00"), minute.ToString("00"));
            _reminderTimePickerEditTextReadableValue.Text = ReadableTimeConverter.Convert(hourOfDay, minute);
        }

        private void SetDatePickerText()
        {
            _reminderDatePickerText = this.FindViewById<EditText>(Resource.Id.reminderDatePickerEditText);
            _reminderDatePickerEditTextReadableValue = this.FindViewById<EditText>(Resource.Id.reminderDatePickerEditTextReadableValue);

            if (!string.IsNullOrEmpty(_reminderDatePickerText.Text))
            {
                var reminderDate = Convert.ToDateTime(_reminderDatePickerText.Text);
                _reminderDatePickerEditTextReadableValue.Text = ReadableDateConverter.Convert(reminderDate);
            }

            _reminderDatePickerEditTextReadableValue.Focusable = false;
            _reminderDatePickerEditTextReadableValue.Click += delegate
            {
                var reminderDate = DateTime.Now;

                if (!string.IsNullOrEmpty(_reminderDatePickerText.Text))
                {
                    reminderDate = Convert.ToDateTime(_reminderDatePickerText.Text);
                }

                DatePickerDialog dialog = new DatePickerDialog(this, this, reminderDate.Year, reminderDate.Month - 1, reminderDate.Day);
                dialog.SetTitle("Date selection");
                dialog.DatePicker.SetMinDate(DateTime.Now);
                dialog.Show();
            };
        }

        private void SetTimePickerText()
        {
            _reminderTimePickerText = this.FindViewById<EditText>(Resource.Id.reminderTimePickerEditText);
            _reminderTimePickerEditTextReadableValue = this.FindViewById<EditText>(Resource.Id.reminderTimePickerEditTextReadableValue);

            int hours = DateTime.Now.Hour;
            int minutes = DateTime.Now.Minute;
            string reminderTime = "";

            if (!string.IsNullOrEmpty(_reminderTimePickerText.Text))
            {
                reminderTime = _reminderTimePickerText.Text;
                var timeElements = reminderTime.Split(':');
                if (timeElements.Length == 2)
                {
                    if (int.TryParse(timeElements[0], out hours))
                    {
                        if (int.TryParse(timeElements[1], out minutes))
                        {
                            _reminderTimePickerEditTextReadableValue.Text = ReadableTimeConverter.Convert(hours, minutes);
                        }
                    }
                }
            }

            _reminderTimePickerEditTextReadableValue.Focusable = false;
            _reminderTimePickerEditTextReadableValue.Click += delegate
            {
                hours = DateTime.Now.Hour;
                minutes = DateTime.Now.Minute;
                reminderTime = "";

                if (!string.IsNullOrEmpty(_reminderTimePickerText.Text))
                {
                    reminderTime = _reminderTimePickerText.Text;
                    var timeElements = reminderTime.Split(':');
                    if (timeElements.Length == 2)
                    {
                        int.TryParse(timeElements[0], out hours);
                        int.TryParse(timeElements[1], out minutes);
                    }
                }

                bool is24HourView = (CultureInfo.CurrentCulture.Name == "fr-FR");

                TimePickerDialog dialog = new TimePickerDialog(this, this, hours, minutes, is24HourView);
                dialog.SetTitle("Time selection");
                dialog.Show();
            };
        }
    }
}