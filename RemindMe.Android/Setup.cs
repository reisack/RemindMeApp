using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using RemindMe.Core.Interfaces;
using RemindMe.Android.Services;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform.Logging;

namespace RemindMe.Android
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
            
        }

        protected override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.None;

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();
            Mvx.RegisterSingleton<IDialogService>(() => new DialogService());
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }
    }
}