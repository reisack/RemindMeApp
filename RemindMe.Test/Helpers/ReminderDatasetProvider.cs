using RemindMe.Core.Models;
using System;
using System.Collections.Generic;

namespace RemindMe.Test.Helpers
{
    public class ReminderDatasetProvider
    {
        private static Lazy<ReminderDatasetProvider> _singletonInstance = new Lazy<ReminderDatasetProvider>(() => new ReminderDatasetProvider());

        public static ReminderDatasetProvider SingletonInstance
        {
            get { return _singletonInstance.Value; }
        }

        public Reminder GetTestReminderWithComment()
        {
            return new Reminder
            {
                Title = "Title test",
                Comment = "Comment test",
                Date = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds()
            };
        }

        public Reminder GetTestReminderWithoutComment()
        {
            return new Reminder
            {
                Title = "Title test",
                Date = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds()
            };
        }

        public Reminder GetReminderWithoutCommentByDefiningTimestamp(DateTimeOffset timestampOffset)
        {
            return new Reminder
            {
                Title = "Title test",
                Date = timestampOffset.ToUnixTimeSeconds()
            };
        }

        public Reminder GetReminderWithoutTitleByDefiningTimestamp(DateTimeOffset timestampOffset)
        {
            return new Reminder
            {
                Comment = "Comment test",
                Date = timestampOffset.ToUnixTimeSeconds()
            };
        }

        public Reminder GetReminderWithoutDateByDefiningTimestamp()
        {
            return new Reminder
            {
                Title = "Title test",
                Comment = "Comment test"
            };
        }

        public List<Reminder> GetListWithPastAndUpcomingReminders()
        {
            return new List<Reminder>
            {
                new Reminder
                {
                    Title = "Title test 1",
                    Comment = "Comment test 1",
                    Date = DateTimeOffset.UtcNow.AddMinutes(-10).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 2",
                    Comment = "Comment test 2",
                    Date = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 3",
                    Comment = "Comment test 3",
                    Date = DateTimeOffset.UtcNow.AddHours(-3).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 4",
                    Comment = "Comment test 4",
                    Date = DateTimeOffset.UtcNow.AddHours(5).ToUnixTimeSeconds()
                }
            };
        }

        public List<Reminder> GetListWithOnlyUpcomingReminders()
        {
            return new List<Reminder>
            {
                new Reminder
                {
                    Title = "Title test 1",
                    Comment = "Comment test 1",
                    Date = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 2",
                    Comment = "Comment test 2",
                    Date = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 3",
                    Comment = "Comment test 3",
                    Date = DateTimeOffset.UtcNow.AddHours(3).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 4",
                    Comment = "Comment test 4",
                    Date = DateTimeOffset.UtcNow.AddHours(5).ToUnixTimeSeconds()
                }
            };
        }

        public List<Reminder> GetListWithOnlyPastReminders()
        {
            return new List<Reminder>
            {
                new Reminder
                {
                    Title = "Title test 1",
                    Comment = "Comment test 1",
                    Date = DateTimeOffset.UtcNow.AddMinutes(-10).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 2",
                    Comment = "Comment test 2",
                    Date = DateTimeOffset.UtcNow.AddMinutes(-30).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 3",
                    Comment = "Comment test 3",
                    Date = DateTimeOffset.UtcNow.AddHours(-3).ToUnixTimeSeconds()
                },
                new Reminder
                {
                    Title = "Title test 4",
                    Comment = "Comment test 4",
                    Date = DateTimeOffset.UtcNow.AddHours(-5).ToUnixTimeSeconds()
                }
            };
        }
    }
}
