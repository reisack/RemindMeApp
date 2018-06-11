using MvvmCross.Core.ViewModels;
using RemindMe.Core.Models;
using RemindMe.Core.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemindMe.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<ReminderListViewModel>();
        }
    }
}
