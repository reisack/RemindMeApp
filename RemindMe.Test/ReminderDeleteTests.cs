using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Fakes;
using RemindMe.Test.Helpers;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderDeleteTests
    {
        private SQLiteConnection _databaseConnection;
        private ReminderDataService _reminderDataService;
        private ReminderDatasetProvider _ReminderDatasetProvider;

        [TestInitialize]
        public void Init()
        {
            _databaseConnection = DatabaseConnectionFake.SingletonInstance.GetConnection();
            _ReminderDatasetProvider = ReminderDatasetProvider.SingletonInstance;
            _reminderDataService = GetReminderDataServiceWithFakes();

            _databaseConnection.CreateTable<Reminder>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _databaseConnection.DropTable<Reminder>();
        }

        [TestMethod]
        public async Task DeleteAReminderThatAlreadyExistsInDatabase()
        {
            Reminder reminder = _ReminderDatasetProvider.GetTestReminderWithComment();
            _databaseConnection.Insert(reminder, typeof(Reminder));

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
            List<Reminder> reminders = _ReminderDatasetProvider.GetListWithPastAndUpcomingReminders();
            _databaseConnection.InsertAll(reminders, typeof(Reminder));

            int numberOfDeletedReminders = await _reminderDataService.DeletePast();
            TableQuery<Reminder> reminderTable = _databaseConnection.Table<Reminder>();
            Reminder reminder2 = reminderTable.FirstOrDefault((x) => x.Title == "Title test 2");
            Reminder reminder4 = reminderTable.FirstOrDefault((x) => x.Title == "Title test 4");

            Assert.AreEqual(2, numberOfDeletedReminders);
            Assert.IsNotNull(reminder2);
            Assert.IsNotNull(reminder4);

        }

        [TestMethod]
        public async Task NoPastRemindersToDelete()
        {
            List<Reminder> reminders = _ReminderDatasetProvider.GetListWithOnlyUpcomingReminders();
            _databaseConnection.InsertAll(reminders, typeof(Reminder));

            int numberOfDeletedReminders = await _reminderDataService.DeletePast();

            Assert.AreEqual(0, numberOfDeletedReminders);
        }

        private ReminderDataService GetReminderDataServiceWithFakes()
        {
            DatabaseConnectionFake connectionService = DatabaseConnectionFake.SingletonInstance;
            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);

            return dataService;
        }
    }
}
