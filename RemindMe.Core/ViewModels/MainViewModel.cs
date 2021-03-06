﻿using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;

namespace RemindMe.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly Lazy<ReminderListViewModel> _remindersListVm;

        public ReminderListViewModel RemindersListVm => _remindersListVm.Value;

        public MainViewModel()
        {
            _remindersListVm = new Lazy<ReminderListViewModel>(Mvx.IocConstruct<ReminderListViewModel>);
            ShowRemindersList();
        }

        public void ShowRemindersList()
        {
            ShowViewModel<ReminderListViewModel>();
        }
    }
}