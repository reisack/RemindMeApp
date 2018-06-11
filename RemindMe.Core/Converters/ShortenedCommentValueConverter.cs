using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RemindMe.Core.Converters
{
    public class ShortenedCommentValueConverter : MvxValueConverter<string, string>
    {
        private const int MESSAGE_LENGTH = 100;

        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            // Removing line breaks
            string newValue = value.Replace(System.Environment.NewLine, " ");

            if (newValue.Length > MESSAGE_LENGTH)
            {
                return newValue.Substring(0, MESSAGE_LENGTH) + "...";
            }
            else
            {
                return newValue;
            }
        }
    }
}
