using System.Collections.Generic;
using System.Threading.Tasks;
using RemindMe.Core.Database;
using RemindMe.Core.Models;

namespace RemindMe.Android.Services
{
    public class ReminderDaemonDataService
    {
        public async Task<IEnumerable<Reminder>> GetRemindersToNotify()
        {
            var db = DatabaseConnection.Instance.GetConnection();

            string query = @"SELECT Id, Title, Comment, Date, AlreadyNotified 
                             FROM Reminder 
                             WHERE Date < strftime('%s','now') 
                             AND AlreadyNotified = 0";

            return await Task.FromResult<IEnumerable<Reminder>>(db.Query<Reminder>(query));
        }

        public async Task SetToNotified(IEnumerable<Reminder> reminders)
        {
            await Task.Run(() =>
            {
                foreach (var reminder in reminders)
                {
                    reminder.AlreadyNotified = 1;
                }

                var db = DatabaseConnection.Instance.GetConnection();
                db.UpdateAll(reminders);
            });
        }
    }
}