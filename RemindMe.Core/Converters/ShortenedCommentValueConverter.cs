using MvvmCross.Platform.Converters;
using RemindMe.Core.Extensions;
using System;
using System.Globalization;

namespace RemindMe.Core.Converters
{
    public class ShortenedCommentValueConverter : MvxValueConverter<string, string>
    {
        private const int MESSAGE_LENGTH = 100;

        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            // Removing line breaks
            string newValue = value.Replace(System.Environment.NewLine, " ");
            newValue = newValue.MergeWhiteSpaces();

            if (newValue.Length > MESSAGE_LENGTH)
            {
                return string.Concat(newValue.Substring(0, MESSAGE_LENGTH),"...");
            }
            else
            {
                return newValue;
            }
        }
    }
}
