using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Fakes;
using RemindMe.Test.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderReadTests
    {
        private ReminderDataService _reminderDataService;

        [TestInitialize]
        public void Init()
        {
            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            db.DropTable<Reminder>();
            db.CreateTable<Reminder>();

            _reminderDataService = GetReminderDataServiceWithMocks();
        }

        [TestMethod]
        public async Task ReturnTwoRemindersToNotifyInFour()
        {
            var reminders = ReminderDatasetProvider.SingletonInstance.GetListWithPastAndUpcomingReminders();

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            var remindersToNotify = await _reminderDataService.GetRemindersToNotify();
            Reminder reminder1 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 1");
            Reminder reminder3 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 3");

            Assert.AreEqual(2, remindersToNotify.Count());
            Assert.IsNotNull(reminder1);
            Assert.IsNotNull(reminder3);
        }

        [TestMethod]
        public async Task ReturnNoReminderToNotify()
        {
            var reminders = ReminderDatasetProvider.SingletonInstance.GetListWithOnlyUpcomingReminders();

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            var remindersToNotify = await _reminderDataService.GetRemindersToNotify();
            var numberOfRemindersToNotify = remindersToNotify.Count();

            Assert.AreEqual(0, numberOfRemindersToNotify);
        }

        [TestMethod]
        public void ReturnANullNextReminderTimestamp()
        {
            var reminders = ReminderDatasetProvider.SingletonInstance.GetListWithOnlyPastReminders();

            long? timestampMustBeNull = _reminderDataService.GetNextReminderTimestamp();

            Assert.IsNull(timestampMustBeNull);
        }

        [TestMethod]
        public void ReturnTheNextReminderTimestamp()
        {
            var reminders = ReminderDatasetProvider.SingletonInstance.GetListWithPastAndUpcomingReminders();

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            long? nextReminderTimestamp = _reminderDataService.GetNextReminderTimestamp();
            Reminder reminder2 = reminders.FirstOrDefault((x) => x.Title == "Title test 2");

            Assert.AreEqual(reminder2.Date, nextReminderTimestamp);
        }

        [TestMethod]
        public async Task ReturnAllRemindersInExpectedOrder()
        {
            var reminders = ReminderDatasetProvider.SingletonInstance.GetListWithPastAndUpcomingReminders();

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            var allReminders = await _reminderDataService.GetAll();
            IList<Reminder> allRemindersList = new List<Reminder>(allReminders);

            Assert.AreEqual("Title test 2", allRemindersList[0].Title);
            Assert.AreEqual("Title test 4", allRemindersList[1].Title);
            Assert.AreEqual("Title test 1", allRemindersList[2].Title);
            Assert.AreEqual("Title test 3", allRemindersList[3].Title);
        }

        private ReminderDataService GetReminderDataServiceWithMocks()
        {
            DatabaseConnectionFake connectionService = DatabaseConnectionFake.SingletonInstance;
            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);

            return dataService;
        }
    }
}
