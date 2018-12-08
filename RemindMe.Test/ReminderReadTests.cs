using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Fakes;
using RemindMe.Test.Helpers;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderReadTests
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
        public async Task ReturnTwoRemindersToNotifyInFour()
        {
            List<Reminder> reminders = _ReminderDatasetProvider.GetListWithPastAndUpcomingReminders();
            _databaseConnection.InsertAll(reminders, typeof(Reminder));

            IEnumerable<Reminder> remindersToNotify = await _reminderDataService.GetRemindersToNotify();
            Reminder reminder1 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 1");
            Reminder reminder3 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 3");

            Assert.AreEqual(2, remindersToNotify.Count());
            Assert.IsNotNull(reminder1);
            Assert.IsNotNull(reminder3);
        }

        [TestMethod]
        public async Task ReturnNoReminderToNotify()
        {
            List<Reminder> reminders = _ReminderDatasetProvider.GetListWithOnlyUpcomingReminders();
            _databaseConnection.InsertAll(reminders, typeof(Reminder));

            IEnumerable<Reminder> remindersToNotify = await _reminderDataService.GetRemindersToNotify();
            int numberOfRemindersToNotify = remindersToNotify.Count();

            Assert.AreEqual(0, numberOfRemindersToNotify);
        }

        [TestMethod]
        public void ReturnANullNextReminderTimestamp()
        {
            List<Reminder> reminders = _ReminderDatasetProvider.GetListWithOnlyPastReminders();

            long? timestampMustBeNull = _reminderDataService.GetNextReminderTimestamp();

            Assert.IsNull(timestampMustBeNull);
        }

        [TestMethod]
        public void ReturnTheNextReminderTimestamp()
        {
            List<Reminder> reminders = _ReminderDatasetProvider.GetListWithPastAndUpcomingReminders();
            _databaseConnection.InsertAll(reminders, typeof(Reminder));

            long? nextReminderTimestamp = _reminderDataService.GetNextReminderTimestamp();
            Reminder reminder2 = reminders.FirstOrDefault((x) => x.Title == "Title test 2");

            Assert.AreEqual(reminder2.Date, nextReminderTimestamp);
        }

        [TestMethod]
        public async Task ReturnAllRemindersInExpectedOrder()
        {
            List<Reminder> reminders = _ReminderDatasetProvider.GetListWithPastAndUpcomingReminders();
            _databaseConnection.InsertAll(reminders, typeof(Reminder));

            IEnumerable<Reminder> allReminders = await _reminderDataService.GetAll();
            List<Reminder> allRemindersList = new List<Reminder>(allReminders);

            Assert.AreEqual("Title test 2", allRemindersList[0].Title);
            Assert.AreEqual("Title test 4", allRemindersList[1].Title);
            Assert.AreEqual("Title test 1", allRemindersList[2].Title);
            Assert.AreEqual("Title test 3", allRemindersList[3].Title);
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
