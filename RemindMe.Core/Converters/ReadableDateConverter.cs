using RemindMe.Core.Extensions;
using System;
using System.Globalization;

namespace RemindMe.Core.Converters
{
    public static class ReadableDateConverter
    {
        public static string Convert(DateTime date)
        {
            string dayOfWeek = date.ToString("dddd")
                .UpperFirstCharIfPossible();

            string month = date.ToString("MMMM");

            if (CultureInfo.CurrentCulture.Name == "en-US")
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
