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
            bool pastEvent = (date < DateTime.Now);

            TimeSpan span = date - DateTime.Now;

            int timeNumber;
            string timeUnit;

            int absDays = Math.Abs(span.Days);
            int absHours = Math.Abs(span.Hours);
            int absMinutes = Math.Abs(span.Minutes);
            int absSeconds = Math.Abs(span.Seconds);

            if (absDays > 365)
            {
                timeNumber = (int)(absDays / NB_DAYS_IN_YEAR);
                timeUnit = "year";
            }
            else if (absDays > 30)
            {
                timeNumber = (int)(absDays / NB_DAYS_IN_MONTH);
                timeUnit = "month";
            }
            else if (absDays >= 1)
            {
                timeNumber = absDays;
                timeUnit = "day";
            }
            else if (absHours >= 1)
            {
                timeNumber = absHours;
                timeUnit = "hour";
            }
            else if (absMinutes >= 1)
            {
                timeNumber = absMinutes;
                timeUnit = "minute";
            }
            else
            {
                timeNumber = absSeconds;
                timeUnit = "second";
            }

            if (timeNumber > 1 && !timeUnit.EndsWith("s"))
            {
                timeUnit += "s";
            }

            if (culture.Name == "en-US")
            {
                if (pastEvent)
                {
                    return string.Format("{0} {1} ago", timeNumber, timeUnit);
                }
                else
                {
                    return string.Format("{0} {1} left", timeNumber, timeUnit);
                }
            }
            else
            {
                if (pastEvent)
                {
                    return string.Format("Il y a {0} {1}", timeNumber, timeUnit);
                }
                else
                {
                    return string.Format("Dans {0} {1}", timeNumber, timeUnit);
                }
            }
        }
    }
}
