using RemindMe.Core.Models;
using System;
using System.Collections.Generic;

namespace RemindMe.Test.Helpers
{
    public static class ReminderDatasetProvider
    {
        public static Reminder GetTestReminderWithComment()
        {
            return new Reminder
            {
                Title = "Title test",
                Comment = "Comment test",
                Date = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds()
            };
        }

        public static Reminder GetTestReminderWithoutComment()
        {
            return new Reminder
            {
                Title = "Title test",
                Date = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds()
            };
        }

        public static Reminder GetReminderWithoutCommentByDefiningTimestamp(DateTimeOffset timestampOffset)
        {
            return new Reminder
            {
                Title = "Title test",
                Date = timestampOffset.ToUnixTimeSeconds()
            };
        }

        public static Reminder GetReminderWithoutTitleByDefiningTimestamp(DateTimeOffset timestampOffset)
        {
            return new Reminder
            {
                Comment = "Comment test",
                Date = timestampOffset.ToUnixTimeSeconds()
            };
        }

        public static Reminder GetReminderWithoutDateByDefiningTimestamp()
        {
            return new Reminder
            {
                Title = "Title test",
                Comment = "Comment test"
            };
        }

        public static List<Reminder> GetListWithPastAndUpcomingReminders()
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

        public static List<Reminder> GetListWithOnlyUpcomingReminders()
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

        public static List<Reminder> GetListWithOnlyPastReminders()
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
