using Android.App;
using Android.Text.Format;
using Java.Util;

namespace RemindMe.Android.Converters
{
    public class ReadableTimeConverter
    {
        public static string Convert(int hours, int minutes)
        {
            // For this date, we only care about hour and minute
            // We set the date on the first of January 2000 as a dummy value
            // This is faster than doing GetYear(), GetMonth(), etc...
            Date date = new Date(2000, 1, 1, hours, minutes, 0);

            if (!DateFormat.Is24HourFormat(Application.Context))
            {
                return DateFormat.Format("h:mm a", date);
                //string amOrPm = "am";

                //if (hours == 0)
                //{
                //    hours = 12;
                //    amOrPm = "am";
                //}
                //else if (hours == 12)
                //{
                //    amOrPm = "pm";
                //}
                //else if (hours > 12)
                //{
                //    hours -= 12;
                //    amOrPm = "pm";
                //}

                //return $"{hours}:{minutes.ToString("00")} {amOrPm}";
            }
            else
            {
                return DateFormat.Format("HH:mm", date);
                //return $"{hours.ToString("00")}:{minutes.ToString("00")}";
            }
        }
    }
}
