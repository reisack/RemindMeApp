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
        private readonly IDialogService _dialogService;

        private ObservableCollection<Reminder> _reminders;

        public event EventHandler OnDeletePastReminders;

        public ObservableCollection<Reminder> Reminders
        {
            get { return _reminders; }
            set
            {
                _reminders = value;
                RaisePropertyChanged(() => Reminders);
            }
        }

        public ReminderListViewModel(IMvxMessenger messenger, IReminderDataService reminderDataService, IDialogService dialogService) : base(messenger)
        {
            _reminderDataService = reminderDataService;
            _dialogService = dialogService;
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

        public MvxCommand DeletePastRemindersCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    bool dialogResponse = await _dialogService.ShowConfirmAsync("This will delete all past reminders, do you want to continue ?", "Delete all past reminders", "Yes", "No");
                    if (dialogResponse)
                    {
                        await _reminderDataService.DeletePast();
                        OnDeletePastReminders?.Invoke(this, new EventArgs());
                        ReloadDataCommand.Execute();
                    }
                });
            }
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
