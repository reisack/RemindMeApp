using System.Globalization;

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

                return $"{hours}:{minutes.ToString("00")} {amOrPm}";
            }
            else
            {
                return $"{hours.ToString("00")}:{minutes.ToString("00")}";
            }
        }
    }
}
