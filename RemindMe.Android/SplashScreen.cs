using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace RemindMe.Android
{
    [Activity(
        MainLauncher = true, 
        Label = "@string/app_name",
        Theme = "@style/TransparentTheme",
        NoHistory = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen() : base(Resource.Layout.Splash)
        {

        }
    }
}