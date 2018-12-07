using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Fakes;
using RemindMe.Test.Helpers;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderSaveTests
    {
        #region Consts
        const string LONG_MESSAGE = @"Lorem ipsum dolor sit amet, 
consectetur adipiscing elit. Suspendisse et augue elementum, 
gravida justo non, lobortis orci. Nullam blandit id eros vitae pretium. 
Praesent eget erat molestie, tempor lorem at, posuere turpis. 
Pellentesque malesuada molestie mi et rutrum. Mauris ac diam purus. 
Phasellus a pharetra justo. Pellentesque egestas tincidunt dui, 
eu ultrices orci vestibulum sed.
Nullam fringilla id est quis eleifend. 
Sed at neque nec lacus rutrum luctus. 
Phasellus nec nunc in est porttitor aliquam vitae in lorem. 
Nunc pellentesque mauris ut vulputate rutrum. Fusce diam nisi, 
viverra in velit sit amet, rhoncus mollis risus. 
Pellentesque nec diam eget lectus egestas tempor ac id tellus. 
Quisque a nulla nisi. Nam ut tellus et mauris pulvinar ullamcorper. 
Sed tempor elit eu lacinia volutpat. Donec posuere vulputate felis, 
quis tincidunt lectus.";

        const string EXPECTED_TITLE = @"Lorem ipsum dolor sit amet, 
consectetur adipisci";

        const string EXPECTED_COMMENT = @"Lorem ipsum dolor sit amet, 
consectetur adipiscing elit. Suspendisse et augue elementum, 
gravida justo non, lobortis orci. Nullam blandit id eros vitae pretium. 
Praesent eget erat molestie, tempor lorem at, posuere turpis. 
Pellentesque malesuada molestie mi et rutrum. Mauris ac diam purus. 
Phasellus a pharetra justo. Pellentesque egestas tincidunt dui, 
eu ultrices orci vestibulum sed.
Nullam fringilla id est quis eleifend. 
Sed at neque nec lacus rutrum luctus. 
Phasellus nec nunc";
        #endregion

        private ReminderDataService _reminderDataService;

        [TestInitialize]
        public void Init()
        {
            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            db.DropTable<Reminder>();
            db.CreateTable<Reminder>();

            _reminderDataService = GetReminderDataServiceWithFakes();
        }

        [TestMethod]
        public async Task CreateReminderWithoutComment()
        {
            Reminder reminder = ReminderDatasetProvider.SingletonInstance.GetTestReminderWithoutComment();

            int numberOfCreatedReminders = await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreEqual(1, numberOfCreatedReminders);
        }

        [TestMethod]
        public async Task CreateReminderWithComment()
        {
            Reminder reminder = ReminderDatasetProvider.SingletonInstance.GetTestReminderWithComment();

            int numberOfCreatedReminders = await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreEqual(1, numberOfCreatedReminders);
        }

        [TestMethod]
        public async Task CreateReminderWithCommentAndUpdateWithoutIt()
        {
            Reminder reminder = ReminderDatasetProvider.SingletonInstance.GetTestReminderWithComment();

            await _reminderDataService.AddOrUpdate(reminder);

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            insertedReminder.Comment = null;
            await _reminderDataService.AddOrUpdate(insertedReminder);
            Reminder updatedReminder = db.Find<Reminder>(insertedReminder.Id);

            Assert.IsNull(updatedReminder.Comment);
        }

        [TestMethod]
        public async Task CreateReminderWithoutCommentAndUpdateWithIt()
        {
            Reminder reminder = ReminderDatasetProvider.SingletonInstance.GetTestReminderWithoutComment();

            await _reminderDataService.AddOrUpdate(reminder);

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            insertedReminder.Comment = "Comment test";
            await _reminderDataService.AddOrUpdate(insertedReminder);
            Reminder updatedReminder = db.Find<Reminder>(insertedReminder.Id);

            Assert.AreEqual("Comment test", updatedReminder.Comment);
        }

        [TestMethod]
        public async Task CreateReminderWithBothTooLongTitleAndComment()
        {
            Reminder reminder = new Reminder
            {
                Title = LONG_MESSAGE,
                Comment = LONG_MESSAGE,
                Date = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds()
            };

            await _reminderDataService.AddOrUpdate(reminder);

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            Assert.AreEqual(EXPECTED_TITLE, insertedReminder.Title);
            Assert.AreEqual(EXPECTED_COMMENT, insertedReminder.Comment);
        }

        [TestMethod]
        public async Task UpdateReminderWithBothTooLongTitleAndComment()
        {
            Reminder reminder = ReminderDatasetProvider.SingletonInstance.GetTestReminderWithComment();

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            db.Insert(reminder, typeof(Reminder));

            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            insertedReminder.Title = LONG_MESSAGE;
            insertedReminder.Comment = LONG_MESSAGE;

            await _reminderDataService.AddOrUpdate(insertedReminder);
            Reminder updatedReminder = db.Find<Reminder>(insertedReminder.Id);

            Assert.AreEqual(EXPECTED_TITLE, updatedReminder.Title);
            Assert.AreEqual(EXPECTED_COMMENT, updatedReminder.Comment);
        }

        [TestMethod]
        public async Task UpdateReminderData()
        {
            Reminder reminder = ReminderDatasetProvider.SingletonInstance.GetTestReminderWithComment();

            long reminderTimestamp = reminder.Date;
            long updatedTimestamp = reminderTimestamp + (60 * 5);

            await _reminderDataService.AddOrUpdate(reminder);

            var db = DatabaseConnectionFake.SingletonInstance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            insertedReminder.Title = "Title test updated";
            insertedReminder.Comment = "Comment test updated";
            insertedReminder.Date = updatedTimestamp;
            insertedReminder.AlreadyNotified = 1;

            await _reminderDataService.AddOrUpdate(insertedReminder);

            Reminder updatedReminder = db.Find<Reminder>(insertedReminder.Id);

            Assert.AreEqual("Title test updated", updatedReminder.Title);
            Assert.AreEqual("Comment test updated", updatedReminder.Comment);
            Assert.AreEqual(updatedTimestamp, updatedReminder.Date);

            // The reminder has to be notified in 15 minutes
            // So, AlreadyNotified has been forced to false (0 in SQLite)
            Assert.AreEqual(0, updatedReminder.AlreadyNotified);
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
