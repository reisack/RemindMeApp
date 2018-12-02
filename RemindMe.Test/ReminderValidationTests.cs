using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Core.ViewModels;
using RemindMe.Test.Mocks;
using System;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderValidationTests
    {
        [TestMethod]
        public void WantToCreateAReminderAlreadyPastSinceTenMinutes()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsFalse(IsReminderValid(reminder));
        }

        [TestMethod]
        public void WantToCreateAReminderAlreadyPastSinceTwoDays()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddHours(1).AddDays(-2);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsFalse(IsReminderValid(reminder));
        }

        [TestMethod]
        public void WantToCreateAReminderThatNotifyInOneHour()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddHours(1);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsTrue(IsReminderValid(reminder));
        }

        [TestMethod]
        public void WantToCreateAReminderWithoutTitle()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Comment = "Comment test",
                Date = timestamp
            };

            Assert.IsFalse(IsReminderValid(reminder));
        }

        [TestMethod]
        public void WantToCreateAReminderWithoutComment()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsTrue(IsReminderValid(reminder));
        }

        [TestMethod]
        public void WantToCreateAReminderWithoutDate()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Comment = "Comment test",
                Date = timestamp
            };

            Assert.IsFalse(IsReminderValid(reminder));
        }

        private ReminderEditViewModel GetReminderViewModelWithMocks()
        {
            DialogMockService dialogService = new DialogMockService();
            DatabaseConnectionMock connectionService = new DatabaseConnectionMock();

            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);
            ReminderEditViewModel viewModel = new ReminderEditViewModel(dataService, dialogService);

            return viewModel;
        }

        private bool IsReminderValid(Reminder reminder)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(reminder.Date);

            ReminderEditViewModel viewModel = GetReminderViewModelWithMocks();
            viewModel.SelectedReminder = reminder;

            if (reminder.Date > 0)
            {
                viewModel.ReminderDay = dateTimeOffset.LocalDateTime;
                viewModel.ReminderTime = viewModel.ReminderDay.Value.ToString("HH:mm");
            }

            PrivateObject reminderViewModelMock = new PrivateObject(viewModel);
            return (bool)reminderViewModelMock.Invoke("Validate");
        }
    }
}
