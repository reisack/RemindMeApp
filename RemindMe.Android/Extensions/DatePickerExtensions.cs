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

namespace RemindMe.Android.Extensions
{
    // https://forums.xamarin.com/discussion/12003/datepickerdialog-with-min-max

    public static class DatePickerExtensions
    {
        public static void SetMinDate(this DatePicker picker, DateTime dt)
        {
            var javaMinDt = new DateTime(1970, 1, 1);
            if (dt.CompareTo(javaMinDt) < 0)
                throw new ArgumentException("Must be >= Java's min DateTime of 1/1970");

            var longVal = dt - javaMinDt;
            picker.MinDate = (long)longVal.TotalMilliseconds;
        }

        public static void SetMaxDate(this DatePicker picker, DateTime dt)
        {
            var javaMinDt = new DateTime(1970, 1, 1);
            if (dt.CompareTo(javaMinDt) < 0)
                throw new ArgumentException("Must be >= Java's min DateTime of 1/1970");

            var longVal = dt - javaMinDt;
            picker.MaxDate = (long)longVal.TotalMilliseconds;
        }
    }
}