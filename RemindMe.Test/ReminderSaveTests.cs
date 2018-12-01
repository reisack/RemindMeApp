using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Core.ViewModels;
using RemindMe.Test.MockServices;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderSaveTests
    {
        [TestMethod]
        public void CreateReminderWithoutComment()
        {
            Reminder reminder = new Reminder
            {
                Title = "Title test",
                Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + (1000 * 60)
            };

            var viewModel = GetReminderViewModelWithMocks();
            viewModel.SelectedReminder = reminder;
            viewModel.SaveCommand.Execute();
        }

        [TestMethod]
        public void CreateReminderWithComment()
        {
            Reminder reminder = new Reminder
            {
                Title = "Title test",
                Comment = "Comment test",
                Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - (1000 * 60)
            };

            var viewModel = GetReminderViewModelWithMocks();
            viewModel.SelectedReminder = reminder;
            viewModel.SaveCommand.Execute();
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
    }
}
