using MvvmCross.Platform.Converters;
using RemindMe.Core.Localization;
using System;
using System.Globalization;

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
                timeUnit = LocalizationManager.GetString("year");
            }
            else if (absDays > 30)
            {
                timeNumber = (int)(absDays / NB_DAYS_IN_MONTH);
                timeUnit = LocalizationManager.GetString("month");
            }
            else if (absDays >= 1)
            {
                timeNumber = absDays;
                timeUnit = LocalizationManager.GetString("day");
            }
            else if (absHours >= 1)
            {
                timeNumber = absHours;
                timeUnit = LocalizationManager.GetString("hour");
            }
            else if (absMinutes >= 1)
            {
                timeNumber = absMinutes;
                timeUnit = LocalizationManager.GetString("minute");
            }
            else
            {
                timeNumber = absSeconds;
                timeUnit = LocalizationManager.GetString("second");
            }

            if (timeNumber > 1 && !timeUnit.EndsWith("s"))
            {
                timeUnit += "s";
            }

            if (pastEvent)
            {
                return string.Format(LocalizationManager.GetString("time_past"), timeNumber, timeUnit);
            }
            else
            {
                return string.Format(LocalizationManager.GetString("time_future"), timeNumber, timeUnit);
            }
        }
    }
}
