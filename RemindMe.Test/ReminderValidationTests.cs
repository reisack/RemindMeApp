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
        public void ValidateAReminderAlreadyPastSinceTenMinutes()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsFalse(IsReminderValid(reminder));
        }

        [TestMethod]
        public void ValidateAReminderAlreadyPastSinceTwoDays()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddHours(1).AddDays(-2);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsFalse(IsReminderValid(reminder));
        }

        [TestMethod]
        public void ValidateAReminderThatNotifyInOneHour()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddHours(1);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsTrue(IsReminderValid(reminder));
        }

        [TestMethod]
        public void ValidateAReminderWithoutTitle()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Comment = "Comment test",
                Date = timestamp
            };

            Assert.IsFalse(IsReminderValid(reminder));
        }

        [TestMethod]
        public void ValidateAReminderWithoutComment()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Title = "Title test",
                Date = timestamp
            };

            Assert.IsTrue(IsReminderValid(reminder));
        }

        [TestMethod]
        public void ValidateAReminderWithoutDate()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
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

            PrivateObject reminderViewModelMock = new PrivateObject(viewModel);
            reminderViewModelMock.Invoke("SetReminderDayAndTime");
            return (bool)reminderViewModelMock.Invoke("Validate");
        }
    }
}
