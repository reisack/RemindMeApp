using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RemindMe.Core.Converters
{
    public class ReadableTimeConverter
    {
        public static string Convert(int hours, int minutes)
        {
            if (CultureInfo.CurrentCulture.Name == "en-US")
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

                return string.Format("{0}:{1} {2}", hours, minutes.ToString("00"), amOrPm);
            }
            else
            {
                return string.Format("{0}:{1}", hours.ToString("00"), minutes.ToString("00"));
            }
        }
    }
}
