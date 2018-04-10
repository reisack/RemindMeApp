using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using RemindMe.Core.Extensions;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Core.ViewModels
{
    public class ReminderListViewModel : BaseViewModel, IReminderListViewModel
    {
        private readonly IReminderDataService _reminderDataService;

        private ObservableCollection<Reminder> _reminders;

        public ObservableCollection<Reminder> Reminders
        {
            get { return _reminders; }
            set
            {
                _reminders = value;
                RaisePropertyChanged(() => Reminders);
            }
        }

        public ReminderListViewModel(IMvxMessenger messenger, IReminderDataService reminderDataService) : base(messenger)
        {
            _reminderDataService = reminderDataService;
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            var remindersCollection = await _reminderDataService.GetAll();
            if (remindersCollection != null)
            {
                Reminders = remindersCollection.ToObservableCollection();
            }
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            ReloadDataCommand.Execute();
        }

        public async void DeletePastReminders()
        {
            await _reminderDataService.DeletePast();
        }

        public MvxCommand ReloadDataCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    Reminders = (await _reminderDataService.GetAll()).ToObservableCollection();
                });
            }
        }

        public MvxCommand AddCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<ReminderEditViewModel>();
                });
            }
        }

        public MvxCommand<Reminder> SelectItemCommand
        {
            get
            {
                return new MvxCommand<Reminder>(selectedReminder =>
                {
                    ShowViewModel<ReminderEditViewModel>(new
                    {
                        id = selectedReminder.Id
                    });
                });
            }
        }
    }
}
