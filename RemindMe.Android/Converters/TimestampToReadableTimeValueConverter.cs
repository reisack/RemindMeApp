using Android.App;
using Android.Text.Format;
using Java.Util;
using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace RemindMe.Android.Converters
{
    public class TimestampToReadableTimeValueConverter : MvxValueConverter<long, string>
    {
        protected override string Convert(long value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(value);
            DateTime localDate = dateTimeOffset.LocalDateTime;

            // For this date, we only care about hour and minute
            // We set the date on the first of January 2000 as a dummy value
            // This is faster than doing GetYear(), GetMonth(), etc...
            Date date = new Date(2000, 1, 1, localDate.Hour, localDate.Minute, 0);

            if (!DateFormat.Is24HourFormat(Application.Context))
            {
                return DateFormat.Format("h:mm a", date);
            }
            else
            {
                return DateFormat.Format("HH:mm", date);
            }
        }
    }
}
