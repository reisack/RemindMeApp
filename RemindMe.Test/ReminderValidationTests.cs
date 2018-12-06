using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Core.ViewModels;
using RemindMe.Test.Fakes;
using RemindMe.Test.Helpers;
using System;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderValidationTests
    {
        [TestMethod]
        public void AReminderAlreadyPastSinceTenMinutesMustBeInvalid()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-10);
            Reminder reminder = ReminderDatasetProvider.Instance.GetReminderWithoutCommentByDefiningTimestamp(dateTimeOffset);

            bool isReminderValid = IsReminderValid(reminder);

            Assert.IsFalse(isReminderValid);
        }

        [TestMethod]
        public void AReminderAlreadyPastSinceFortySevenHoursMustBeInvalid()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddHours(1).AddDays(-2);
            Reminder reminder = ReminderDatasetProvider.Instance.GetReminderWithoutCommentByDefiningTimestamp(dateTimeOffset);

            bool isReminderValid = IsReminderValid(reminder);

            Assert.IsFalse(isReminderValid);
        }

        [TestMethod]
        public void AReminderThatNotifyInOneHourMustBeValid()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddHours(1);
            Reminder reminder = ReminderDatasetProvider.Instance.GetReminderWithoutCommentByDefiningTimestamp(dateTimeOffset);

            bool isReminderValid = IsReminderValid(reminder);

            Assert.IsTrue(isReminderValid);
        }

        [TestMethod]
        public void AReminderWithoutTitleMustBeInvalid()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            Reminder reminder = ReminderDatasetProvider.Instance.GetReminderWithoutTitleByDefiningTimestamp(dateTimeOffset);

            bool isReminderValid = IsReminderValid(reminder);

            Assert.IsFalse(isReminderValid);
        }

        [TestMethod]
        public void AReminderWithoutCommentMustBeValid()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(5);
            Reminder reminder = ReminderDatasetProvider.Instance.GetReminderWithoutCommentByDefiningTimestamp(dateTimeOffset);

            bool isReminderValid = IsReminderValid(reminder);

            Assert.IsTrue(isReminderValid);
        }

        [TestMethod]
        public void AReminderWithoutDateMustBeInvalid()
        {
            Reminder reminder = ReminderDatasetProvider.Instance.GetReminderWithoutDateByDefiningTimestamp();

            bool isReminderValid = IsReminderValid(reminder);

            Assert.IsFalse(isReminderValid);
        }

        private ReminderEditViewModel GetReminderViewModelWithMocks()
        {
            DialogServiceDummy dialogService = new DialogServiceDummy();
            DatabaseConnectionFake connectionService = DatabaseConnectionFake.Instance;

            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);
            ReminderEditViewModel viewModel = new ReminderEditViewModel(dataService, dialogService);

            return viewModel;
        }

        private bool IsReminderValid(Reminder reminder)
        {
            ReminderEditViewModel viewModel = GetReminderViewModelWithMocks();
            viewModel.SelectedReminder = reminder;

            PrivateObject reminderViewModelMock = new PrivateObject(viewModel);
            reminderViewModelMock.Invoke("SetReminderDayAndTime");
            return (bool)reminderViewModelMock.Invoke("Validate");
        }
    }
}
