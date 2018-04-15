using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Transitions;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
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