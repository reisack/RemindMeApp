using MvvmCross.Platform.Converters;
using System;
using System.Globalization;
using RemindMe.Core.Extensions;

namespace RemindMe.Core.Converters
{
    public class TimestampToReadableDateValueConverter : MvxValueConverter<long, string>
    {
        protected override string Convert(long value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(value);
            DateTime date = dateTimeOffset.LocalDateTime;

            string dayOfWeek = date.ToString("dddd")
                .UpperFirstCharIfPossible();

            string month = date.ToString("MMM");

            if (culture.Name == "en-US")
            {
                return $"{dayOfWeek}, {month} {date.Day}, {date.Year}";
            }
            else
            {
                return $"{dayOfWeek} {date.Day} {month} {date.Year}";
            }
        }
    }
}
