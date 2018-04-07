using System;
using System.Collections.Generic;
using System.Text;

namespace RemindMe.Core.Converters
{
    public static class ReadableDateConverter
    {
        private static Dictionary<int, string> _monthReadable = new Dictionary<int, string>
        {
            { 1, "January" }, {2, "February" }, {3, "March" }, {4, "April" }, {5, "May" },
            {6, "June" }, {7, "July" }, {8, "August" }, {9, "September" }, {10, "October" },
            {11, "November" }, {12, "December" }
        };

        public static string Convert(DateTime date)
        {
            string dayOfWeek = date.DayOfWeek.ToString();
            string month = _monthReadable[date.Month];

            return string.Format("{0}, {1} {2}, {3}", dayOfWeek, month, date.Day, date.Year);
            
        }
    }
}
