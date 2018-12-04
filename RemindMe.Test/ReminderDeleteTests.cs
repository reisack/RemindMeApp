using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Mocks;
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
            var db = DatabaseConnectionMock.Instance.GetConnection();
            db.DropTable<Reminder>();
            db.CreateTable<Reminder>();

            _reminderDataService = GetReminderDataServiceWithMocks();
        }

        [TestMethod]
        public async Task DeleteAReminderThatAlreadyExistsInDatabaseMustWork()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp = dateTimeOffset.ToUnixTimeSeconds();

            Reminder reminder = new Reminder
            {
                Title = "Title test",
                Comment = "Comment test",
                Date = timestamp
            };

            var db = DatabaseConnectionMock.Instance.GetConnection();
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
        public async Task DeletePastRemindersOnly()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp1 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-30);
            long timestamp2 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddHours(-3);
            long timestamp3 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddHours(5);
            long timestamp4 = dateTimeOffset.ToUnixTimeSeconds();

            List<Reminder> reminders = new List<Reminder>
            {
                new Reminder
                {
                    Title = "Title test 1",
                    Comment = "Comment test 1",
                    Date = timestamp1
                },
                new Reminder
                {
                    Title = "Title test 2",
                    Comment = "Comment test 2",
                    Date = timestamp2
                },
                new Reminder
                {
                    Title = "Title test 3",
                    Comment = "Comment test 3",
                    Date = timestamp3
                },
                new Reminder
                {
                    Title = "Title test 4",
                    Comment = "Comment test 4",
                    Date = timestamp4
                }
            };

            var db = DatabaseConnectionMock.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            int numberOfDeletedReminders = await _reminderDataService.DeletePast();
            int numberOfRemindersLeft = db.Table<Reminder>().Count();

            Assert.AreEqual(2, numberOfDeletedReminders);
            Assert.AreEqual(2, numberOfRemindersLeft);

        }

        private ReminderDataService GetReminderDataServiceWithMocks()
        {
            DatabaseConnectionMock connectionService = DatabaseConnectionMock.Instance;
            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);

            return dataService;
        }
    }
}
