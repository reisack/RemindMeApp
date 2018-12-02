﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;
using RemindMe.Core.Services;
using RemindMe.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var db = DatabaseConnectionMock.Instance.GetConnection();
            db.DropTable<Reminder>();
            db.CreateTable<Reminder>();

            _reminderDataService = GetReminderDataServiceWithMocks();
        }

        [TestMethod]
        public async Task SomeRemindersToNotify()
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

            var remindersToNotify = await _reminderDataService.GetRemindersToNotify();
            Reminder reminder2 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 2");
            Reminder reminder3 = remindersToNotify.FirstOrDefault((x) => x.Title == "Title test 3");

            Assert.AreEqual(2, remindersToNotify.Count());
            Assert.IsNotNull(reminder2);
            Assert.IsNotNull(reminder3);
        }

        [TestMethod]
        public async Task NoReminderToNotify()
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

            var db = DatabaseConnectionMock.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            var remindersToNotify = await _reminderDataService.GetRemindersToNotify();

            Assert.AreEqual(0, remindersToNotify.Count());
        }

        [TestMethod]
        public void GetNextReminderTimestampIsNull()
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

            long? timestampShouldBeNull = _reminderDataService.GetNextReminderTimestamp();

            Assert.IsNull(timestampShouldBeNull);
        }

        [TestMethod]
        public void GetNextReminderTimestamp()
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

            var db = DatabaseConnectionMock.Instance.GetConnection();
            db.InsertAll(reminders, typeof(Reminder));

            long? nextReminderTimestamp = _reminderDataService.GetNextReminderTimestamp();

            Assert.AreEqual(timestamp2, nextReminderTimestamp);
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
