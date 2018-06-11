using System.Globalization;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Visibility;

namespace RemindMe.Android.Converters
{
    // Impossible to have a better code, we need this converter as well as IntToVisibilityValueConverter
    public class IntToInvertedVisibilityValueConverter : MvxBaseVisibilityValueConverter<int>
    {
        protected override MvxVisibility Convert(int value, object parameter, CultureInfo culture)
        {
            return (value == 1) ? MvxVisibility.Visible : MvxVisibility.Hidden;
        }
    }
}