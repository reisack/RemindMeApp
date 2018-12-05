using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Fakes;
using System;
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
            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.DropTable<Reminder>();
            db.CreateTable<Reminder>();

            _reminderDataService = GetReminderDataServiceWithMocks();
        }

        [TestMethod]
        public async Task ReturnTwoRemindersToNotifyInFour()
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

            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            var remindersToNotify = await _reminderDataService.GetRemindersToNotify();
            Reminder reminder2 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 2");
            Reminder reminder3 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 3");

            Assert.AreEqual(2, remindersToNotify.Count());
            Assert.IsNotNull(reminder2);
            Assert.IsNotNull(reminder3);
        }

        [TestMethod]
        public async Task ReturnNoReminderToNotify()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(10);
            long timestamp1 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(30);
            long timestamp2 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddHours(3);
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

            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            var remindersToNotify = await _reminderDataService.GetRemindersToNotify();
            var numberOfRemindersToNotify = remindersToNotify.Count();

            Assert.AreEqual(0, numberOfRemindersToNotify);
        }

        [TestMethod]
        public void ReturnANullNextReminderTimestamp()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-10);
            long timestamp1 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-30);
            long timestamp2 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddHours(-3);
            long timestamp3 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddHours(-5);
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

            long? timestampMustBeNull = _reminderDataService.GetNextReminderTimestamp();

            Assert.IsNull(timestampMustBeNull);
        }

        [TestMethod]
        public void ReturnTheNextReminderTimestamp()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-10);
            long timestamp1 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(30);
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

            var db = DatabaseConnectionFake.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            long? nextReminderTimestamp = _reminderDataService.GetNextReminderTimestamp();

            Assert.AreEqual(timestamp2, nextReminderTimestamp);
        }

        [TestMethod]
        public async Task ReturnAllRemindersInExpectedOrder()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(-10);
            long timestamp1 = dateTimeOffset.ToUnixTimeSeconds();
            dateTimeOffset = DateTimeOffset.UtcNow.AddMinutes(30);
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

            var db = DatabaseConnectionFake.Instance.GetConnection();
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
            DatabaseConnectionFake connectionService = DatabaseConnectionFake.Instance;
            ReminderRepository repository = new ReminderRepository(connectionService);
            ReminderDataService dataService = new ReminderDataService(repository);

            return dataService;
        }
    }
}
