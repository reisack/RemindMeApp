using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Views;
using RemindMe.Core.ViewModels;

namespace RemindMe.Android.Views
{
    [Activity(Label = "Edit", MainLauncher = true)]
    public class ReminderEditView : MvxActivity, DatePickerDialog.IOnDateSetListener, TimePickerDialog.IOnTimeSetListener
    {
        private EditText _reminderDatePickerText;
        private EditText _reminderTimePickerText;

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
            _reminderDatePickerText.Text = new DateTime(year, month, dayOfMonth).ToString();
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            _reminderTimePickerText.Text = string.Format("{0}:{1}", hourOfDay.ToString("00"), minute.ToString("00"));
        }

        private void SetDatePickerText()
        {
            _reminderDatePickerText = this.FindViewById<EditText>(Resource.Id.reminderDatePickerEditText);
            _reminderDatePickerText.Focusable = false;
            _reminderDatePickerText.Click += delegate
            {
                var reminderDate = DateTime.Now;

                if (!string.IsNullOrEmpty(_reminderDatePickerText.Text))
                {
                    reminderDate = Convert.ToDateTime(_reminderDatePickerText.Text);
                }

                DatePickerDialog dialog = new DatePickerDialog(this, this, reminderDate.Year, reminderDate.Month - 1, reminderDate.Day);
                dialog.SetTitle("Date selection");
                dialog.Show();
            };
        }

        private void SetTimePickerText()
        {
            _reminderTimePickerText = this.FindViewById<EditText>(Resource.Id.reminderTimePickerEditText);
            _reminderTimePickerText.Focusable = false;
            _reminderTimePickerText.Click += delegate
            {
                int hours = DateTime.Now.Hour;
                int minutes = DateTime.Now.Minute;
                string reminderTime = "";

                if (!string.IsNullOrEmpty(_reminderTimePickerText.Text))
                {
                    reminderTime = _reminderDatePickerText.Text;
                    var timeElements = reminderTime.Split(':');
                    if (timeElements.Length == 2)
                    {
                        int.TryParse(timeElements[0], out hours);
                        int.TryParse(timeElements[1], out minutes);
                    }
                }

                TimePickerDialog dialog = new TimePickerDialog(this, this, hours, minutes, true);
                dialog.SetTitle("Time selection");
                dialog.Show();
            };
        }
    }
}