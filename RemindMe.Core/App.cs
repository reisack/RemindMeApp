using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using System.Globalization;

namespace RemindMe.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            InitCultureInfo();

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            
            RegisterAppStart(new AppStart());
        }

        private void InitCultureInfo()
        {
            CultureInfo cultureInfo = null;
            if (CultureInfo.CurrentCulture.Name.Contains("fr"))
            {
                cultureInfo = new CultureInfo("fr-FR");
            }
            else
            {
                cultureInfo = new CultureInfo("en-US");
            }

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

    }
}
