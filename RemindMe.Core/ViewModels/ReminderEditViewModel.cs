using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmValidation;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using RemindMe.Core.Extensions;
using RemindMe.Core.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Core.ViewModels
{
    public class ReminderEditViewModel : BaseViewModel, IReminderEditViewModel
    {
        private readonly IReminderDataService _reminderDataService;
        private readonly IDialogService _dialogService;

        private ObservableDictionary<string, string> _errors;

        private long? _reminderId;
        private Reminder _selectedReminder;

        private DateTime? _reminderDay;
        private string _reminderTime;
        

        public bool IsUpdateMode
        {
            get { return (_reminderId.HasValue && _reminderId.Value > 0); }
        }

        public ObservableDictionary<string, string> Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                RaisePropertyChanged(() => Errors);
            }
        }

        public Reminder SelectedReminder
        {
            get { return _selectedReminder; }
            set
            {
                _selectedReminder = value;
                RaisePropertyChanged(() => SelectedReminder);
            }
        }

        public DateTime? ReminderDay
        {
            get { return _reminderDay; }
            set
            {
                _reminderDay = value;
                RaisePropertyChanged(() => ReminderDay);
            }
        }

        public string ReminderTime
        {
            get { return _reminderTime; }
            set
            {
                _reminderTime = value;
                RaisePropertyChanged(() => ReminderTime);
            }
        }

        public ReminderEditViewModel(IMvxMessenger messenger, IReminderDataService reminderDataService, IDialogService dialogService) : base(messenger)
        {
            _reminderId = null;
            _reminderDataService = reminderDataService;
            _dialogService = dialogService;
            _errors = new ObservableDictionary<string, string>();
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            if (_reminderId.HasValue && _reminderId.Value > 0)
            {
                SelectedReminder = await _reminderDataService.Get(_reminderId.Value);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(SelectedReminder.Date);
                ReminderDay = dateTimeOffset.LocalDateTime;
                ReminderTime = ReminderDay.Value.ToString("HH:mm");
            }
            else
            {
                SelectedReminder = new Reminder();
            }
        }

        public void Init(long id)
        {
            _reminderId = id;
        }

        public MvxCommand DeleteCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (_reminderId.HasValue)
                    {
                        bool dialogResponse = await _dialogService.ShowConfirmAsync("This will delete the reminder, do you want to continue ?", "Delete reminder", "Yes", "No");
                        if (dialogResponse)
                        {
                            await _reminderDataService.Delete(_reminderId.Value);
                            Close(this);
                        }
                    }
                });
            }
        }

        public MvxCommand SaveCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (Validate())
                    {
                        _reminderDataService.AddOrUpdate(_selectedReminder);
                        Close(this);
                    }
                });
            }
        }

        private bool Validate()
        {
            ValidationHelper validator = new ValidationHelper();

            validator.AddRequiredRule(() => ReminderDay, "Date is required");
            validator.AddRequiredRule(() => ReminderTime, "Time is required");
            validator.AddRequiredRule(() => SelectedReminder.Title, "Title is required");

            validator.AddRule("MinimumDate", () =>
            {
                bool condition = true;

                if (_reminderDay.HasValue && !string.IsNullOrEmpty(_reminderTime))
                {
                    var day = _reminderDay.Value;

                    var timeElements = _reminderTime.Split(':');
                    if (timeElements.Length >= 2)
                    {
                        int hour = -1, minute = -1;
                        if (int.TryParse(timeElements[0], out hour))
                        {
                            if (int.TryParse(timeElements[1], out minute))
                            {
                                DateTime newDate = new DateTime(day.Year, day.Month, day.Day, hour, minute, 0, DateTimeKind.Local);
                                newDate = newDate.ToUniversalTime();

                                long unixDateTime = ((DateTimeOffset)newDate).ToUnixTimeSeconds();
                                _selectedReminder.Date = unixDateTime;

                                condition = _selectedReminder.Date > ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                            }
                        }
                    }
                }

                return RuleResult.Assert(condition, "Date and time must not be past ");
            });

            var result = validator.ValidateAll();
            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }
    }
}
