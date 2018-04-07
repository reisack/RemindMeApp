using System;
using System.Collections.Generic;
using System.Text;

namespace RemindMe.Core.Converters
{
    public class ReadableTimeConverter
    {
        public static string Convert(int hours, int minutes)
        {
            return string.Format("{0}:{1}", hours.ToString("00"), minutes.ToString("00"));
        }
    }
}
