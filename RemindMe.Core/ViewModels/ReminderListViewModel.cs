using MvvmCross.Core.ViewModels;
using RemindMe.Core.Extensions;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Localization;
using RemindMe.Core.Models;
using System;
using System.Collections.ObjectModel;
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

        public ReminderListViewModel(IReminderDataService reminderDataService, IDialogService dialogService) : base()
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
                    bool dialogResponse = await _dialogService.ShowConfirmAsync(LocalizationManager.GetString("delete_past_reminders_confirm_message"), LocalizationManager.GetString("delete_past_reminders_confirm_title"), LocalizationManager.GetString("yes"), LocalizationManager.GetString("no"));
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
