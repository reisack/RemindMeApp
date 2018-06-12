using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace RemindMe.Core.Converters
{
    public class TimestampToReadableTimeValueConverter : MvxValueConverter<long, string>
    {
        protected override string Convert(long value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(value);
            DateTime date = dateTimeOffset.LocalDateTime;

            int hours = date.Hour;
            int minutes = date.Minute;

            if (culture.Name == "en-US")
            {
                string amOrPm = "am";

                if (hours == 0)
                {
                    hours = 12;
                    amOrPm = "am";
                }
                else if (hours == 12)
                {
                    amOrPm = "pm";
                }
                else if (hours > 12)
                {
                    hours -= 12;
                    amOrPm = "pm";
                }

                return $"{hours}:{minutes.ToString("00")} {amOrPm}";
            }
            else
            {
                return $"{hours.ToString("00")}:{minutes.ToString("00")}";
            }
        }
    }
}
