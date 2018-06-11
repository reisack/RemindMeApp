using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RemindMe.Core.Converters
{
    public static class ReadableDateConverter
    {
        public static string Convert(DateTime date)
        {
            string dayOfWeek = date.ToString("dddd");
            string month = date.ToString("MMMM");

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                return string.Format("{0}, {1} {2}, {3}", dayOfWeek, month, date.Day, date.Year);
            }
            else
            {
                return string.Format("{0} {1} {2} {3}", dayOfWeek, date.Day, month, date.Year);
            }

        }
    }
}
