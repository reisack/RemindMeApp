using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.Messenger;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Core.Tools;
using System;
using System.Resources;
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
