using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.Messenger;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Repositories;
using RemindMe.Core.Localization;
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

            Mvx.RegisterSingleton<IMvxTextProvider>
                (new ResxTextProvider(Strings.ResourceManager));

            RegisterAppStart(new AppStart());
        }

        private void InitCultureInfo()
        {
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
