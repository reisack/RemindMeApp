using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RemindMe.Core.Converters
{
    public class TimestampToTimeLeftValueConverter : MvxValueConverter<long, string>
    {
        private const float NB_DAYS_IN_YEAR = 365.2425f;
        private const float NB_DAYS_IN_MONTH = 30.436875f;

        protected override string Convert(long value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(value);
            DateTime date = dateTimeOffset.LocalDateTime;

            if (date < DateTime.Now)
            {
                return "Past event";
            }

            TimeSpan span = date - DateTime.Now;

            int timeNumber;
            string timeUnit;

            if (span.Days >= 365)
            {
                timeNumber = (int)(span.Days / NB_DAYS_IN_YEAR);
                timeUnit = "year";
            }
            else if (span.Days >= 30)
            {
                timeNumber = (int)(span.Days / NB_DAYS_IN_MONTH);
                timeUnit = "month";
            }
            else if (span.Days >= 1)
            {
                timeNumber = span.Days;
                timeUnit = "day";
            }
            else if (span.Hours >= 1)
            {
                timeNumber = span.Hours;
                timeUnit = "hour";
            }
            else if (span.Minutes >= 1)
            {
                timeNumber = span.Minutes;
                timeUnit = "minute";
            }
            else
            {
                timeNumber = span.Seconds;
                timeUnit = "second";
            }

            if (timeNumber > 1 && !timeUnit.EndsWith("s"))
            {
                timeUnit += "s";
            }

            if (culture.Name == "en-US")
            {
                return string.Format("{0} {1} left", timeNumber, timeUnit);
            }
            else
            {
                return string.Format("Dans {0} {1}", timeNumber, timeUnit);
            }
        }
    }
}
