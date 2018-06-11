using System.Globalization;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Visibility;

namespace RemindMe.Android.Converters
{
    // Impossible to have a better code, we need this converter as well as IntToInvertedVisibilityValueConverter
    public class IntToVisibilityValueConverter : MvxBaseVisibilityValueConverter<int>
    {
        protected override MvxVisibility Convert(int value, object parameter, CultureInfo culture)
        {
            return (value == 0) ? MvxVisibility.Visible : MvxVisibility.Hidden;
        }
    }
}