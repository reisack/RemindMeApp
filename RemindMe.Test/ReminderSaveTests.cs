using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Mocks;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderSaveTests
    {
        private ReminderDataService _reminderDataService;

        [TestInitialize]
        public void Init()
        {
            var db = DatabaseConnectionMock.Instance.GetConnection();
            db.DropTable<Reminder>();
            db.CreateTable<Reminder>();

            _reminderDataService = GetReminderDataServiceWithMocks();
        }

        [TestMethod]
        public async Task CreateReminderWithoutComment()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Date = timestamp
            };

            await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreNotEqual(0, reminder.Id);

            var db = DatabaseConnectionMock.Instance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            Assert.IsNotNull(insertedReminder);
            Assert.AreEqual("Title test", insertedReminder.Title);
            Assert.IsNull(insertedReminder.Comment);
            Assert.AreEqual(timestamp, insertedReminder.Date);
            Assert.AreEqual(0, insertedReminder.AlreadyNotified);
        }

        [TestMethod]
        public async Task CreateReminderWithComment()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Comment = "Comment test",
                Date = timestamp
            };

            await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreNotEqual(0, reminder.Id);

            var db = DatabaseConnectionMock.Instance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            Assert.IsNotNull(insertedReminder);
            Assert.AreEqual("Title test", insertedReminder.Title);
            Assert.AreEqual("Comment test", insertedReminder.Comment);
            Assert.AreEqual(timestamp, insertedReminder.Date);
            Assert.AreEqual(0, insertedReminder.AlreadyNotified);
        }

        [TestMethod]
        public async Task CreateReminderWithCommentAndUpdateWithoutIt()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Comment = "Comment test",
                Date = timestamp
            };

            await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreNotEqual(0, reminder.Id);

            long insertedId = reminder.Id;

            var db = DatabaseConnectionMock.Instance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            insertedReminder.Comment = null;
            await _reminderDataService.AddOrUpdate(insertedReminder);

            // Id didn't change, reminder has been updated
            Assert.AreEqual(insertedId, insertedReminder.Id);

            Reminder updatedReminder = db.Find<Reminder>(insertedReminder.Id);

            Assert.IsNotNull(updatedReminder);
            Assert.AreEqual("Title test", updatedReminder.Title);
            Assert.IsNull(updatedReminder.Comment);
            Assert.AreEqual(timestamp, updatedReminder.Date);
            Assert.AreEqual(0, updatedReminder.AlreadyNotified);
        }

        [TestMethod]
        public async Task CreateReminderWithoutCommentAndUpdateWithIt()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Date = timestamp
            };

            await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreNotEqual(0, reminder.Id);

            long insertedId = reminder.Id;

            var db = DatabaseConnectionMock.Instance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            insertedReminder.Comment = "Comment test";
            await _reminderDataService.AddOrUpdate(insertedReminder);

            // Id didn't change, reminder has been updated
            Assert.AreEqual(insertedId, insertedReminder.Id);

            Reminder updatedReminder = db.Find<Reminder>(insertedReminder.Id);

            Assert.IsNotNull(updatedReminder);
            Assert.AreEqual("Title test", updatedReminder.Title);
            Assert.AreEqual("Comment test", updatedReminder.Comment);
            Assert.AreEqual(timestamp, updatedReminder.Date);
            Assert.AreEqual(0, updatedReminder.AlreadyNotified);
        }

        [TestMethod]
        public async Task CreateReminderWithBothTooLongTitleAndComment()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            string longMessage = @"Lorem ipsum dolor sit amet, 
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

            string assertedTitle = @"Lorem ipsum dolor sit amet, 
consectetur adipisci";

            string assertedComment = @"Lorem ipsum dolor sit amet, 
consectetur adipiscing elit. Suspendisse et augue elementum, 
gravida justo non, lobortis orci. Nullam blandit id eros vitae pretium. 
Praesent eget erat molestie, tempor lorem at, posuere turpis. 
Pellentesque malesuada molestie mi et rutrum. Mauris ac diam purus. 
Phasellus a pharetra justo. Pellentesque egestas tincidunt dui, 
eu ultrices orci vestibulum sed.
Nullam fringilla id est quis eleifend. 
Sed at neque nec lacus rutrum luctus. 
Phasellus nec nunc";

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = longMessage,
                Comment = longMessage,
                Date = timestamp
            };

            await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreNotEqual(0, reminder.Id);

            var db = DatabaseConnectionMock.Instance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            Assert.IsNotNull(insertedReminder);
            Assert.AreEqual(assertedTitle, insertedReminder.Title);
            Assert.AreEqual(assertedComment, insertedReminder.Comment);
        }

        [TestMethod]
        public async Task ReminderDataCanBeUpdated()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();
            long updatedTimestamp = timestamp + (60 * 5);

            Reminder reminder = new Reminder
            {
                Id = 0,
                Title = "Title test",
                Comment = "Comment test",
                Date = timestamp
            };

            await _reminderDataService.AddOrUpdate(reminder);

            Assert.AreNotEqual(0, reminder.Id);

            long insertedId = reminder.Id;

            var db = DatabaseConnectionMock.Instance.GetConnection();
            Reminder insertedReminder = db.Find<Reminder>(reminder.Id);

            insertedReminder.Title = "Title test updated";
            insertedReminder.Comment = "Comment test updated";
            insertedReminder.Date = updatedTimestamp;
            insertedReminder.AlreadyNotified = 1;

            await _reminderDataService.AddOrUpdate(insertedReminder);

            // Id didn't change, reminder has been updated
            Assert.AreEqual(insertedId, insertedReminder.Id);

            Reminder updatedReminder = db.Find<Reminder>(insertedReminder.Id);

            Assert.IsNotNull(updatedReminder);
            Assert.AreEqual("Title test updated", updatedReminder.Title);
            Assert.AreEqual("Comment test updated", updatedReminder.Comment);
            Assert.AreEqual(updatedTimestamp, updatedReminder.Date);

            // The reminder has to be notified in 15 minutes
            // So, AlreadyNotified has been forced to false (0 in SQLite)
            Assert.AreEqual(0, updatedReminder.AlreadyNotified);
        }

        private ReminderDataService GetReminderDataServiceWithMocks()
        {
            DatabaseConnectionMock connectionService = new DatabaseConnectionMock();
            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);

            return dataService;
        }
    }
}
