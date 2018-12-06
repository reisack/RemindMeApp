using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Fakes;
using RemindMe.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderDeleteTests
    {
        private ReminderDataService _reminderDataService;

        [TestInitialize]
        public void Init()
        {
            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.DropTable<Reminder>();
            db.CreateTable<Reminder>();

            _reminderDataService = GetReminderDataServiceWithMocks();
        }

        [TestMethod]
        public async Task DeleteAReminderThatAlreadyExistsInDatabase()
        {
            Reminder reminder = ReminderDatasetProvider.GetTestReminderWithComment();

            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.Insert(reminder, typeof(Reminder));

            int numberOfDeletedReminders = await _reminderDataService.Delete(reminder.Id);

            Assert.AreEqual(1, numberOfDeletedReminders);
        }

        [TestMethod]
        public async Task DeleteAReminderThatNotExistsInDatabaseMustNotThrowAnException()
        {
            int numberOfDeletedReminders = await _reminderDataService.Delete(123);

            Assert.AreEqual(0, numberOfDeletedReminders);
        }

        [TestMethod]
        public async Task DeleteTwoPastRemindersInFour()
        {
            var reminders = ReminderDatasetProvider.GetListWithPastAndUpcomingReminders();

            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            int numberOfDeletedReminders = await _reminderDataService.DeletePast();
            var reminderTable = db.Table<Reminder>();
            Reminder reminder2 = reminderTable.FirstOrDefault((x) => x.Title == "Title test 2");
            Reminder reminder4 = reminderTable.FirstOrDefault((x) => x.Title == "Title test 4");

            Assert.AreEqual(2, numberOfDeletedReminders);
            Assert.IsNotNull(reminder2);
            Assert.IsNotNull(reminder4);

        }

        [TestMethod]
        public async Task NoPastRemindersToDelete()
        {
            var reminders = ReminderDatasetProvider.GetListWithOnlyUpcomingReminders();

            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            int numberOfDeletedReminders = await _reminderDataService.DeletePast();
            Assert.AreEqual(0, numberOfDeletedReminders);
        }

        private ReminderDataService GetReminderDataServiceWithMocks()
        {
            DatabaseConnectionFake connectionService = DatabaseConnectionFake.Instance;
            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);

            return dataService;
        }
    }
}
