using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RemindMe.Core.Database;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Core.ViewModels;
using System.Globalization;

namespace RemindMe.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            InitCultureInfo();

            Mvx.RegisterType<IConnectionService, DatabaseConnection>();
            Mvx.RegisterType<IReminderRepository, ReminderRepository>();
            Mvx.RegisterType<IReminderDataService, ReminderDataService>();
            Mvx.RegisterType<IReminderListViewModel, ReminderListViewModel>();
            Mvx.RegisterType<IReminderEditViewModel, ReminderEditViewModel>();

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
