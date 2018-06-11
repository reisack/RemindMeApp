using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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