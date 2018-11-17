using RemindMe.Core.Database;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindMe.Core.Repositories
{
    public class ReminderRepository : BaseRepository, IReminderRepository
    {
        public ReminderRepository()
        {
        }

        public async Task AddOrUpdate(Reminder reminder)
        {
            await Task.Run(() =>
            {
                var db = DatabaseConnection.Instance.GetConnection();
                var existingReminder = db.Find<Reminder>(reminder.Id);
                if (existingReminder != null)
                {
                    db.Update(reminder, typeof(Reminder));
                }
                else
                {
                    db.Insert(reminder, typeof(Reminder));
                }
            });
        }

        public async Task Delete(long id)
        {
            await Task.Run(() =>
            {
                var db = DatabaseConnection.Instance.GetConnection();
                db.Delete<Reminder>(id);
            });
        }

        public async Task DeletePast()
        {
            await Task.Run(() =>
            {
                var db = DatabaseConnection.Instance.GetConnection();
                string query = @"DELETE FROM Reminder WHERE Date < strftime('%s','now')";
                db.Execute(query);
            });
        }

        public async Task<Reminder> Get(long id)
        {
            var db = DatabaseConnection.Instance.GetConnection();

            return await Task.FromResult<Reminder>(db.Find<Reminder>(id));
        }

        public async Task<IEnumerable<Reminder>> GetAll()
        {
            var db = DatabaseConnection.Instance.GetConnection();

            string query = @"SELECT Id, Title, Comment, Date, AlreadyNotified FROM
                             (
                                SELECT Id, Title, Comment, Date, AlreadyNotified
                                FROM Reminder 
                                WHERE Date >= strftime('%s','now')
                                ORDER BY Date ASC, Title ASC
                             ) AS T1
                             UNION ALL
                             SELECT Id, Title, Comment, Date, AlreadyNotified FROM
                             (
                                SELECT Id, Title, Comment, Date, AlreadyNotified
                                FROM Reminder 
                                WHERE Date < strftime('%s','now')
                                ORDER BY Date DESC, Title ASC
                             ) AS T2 ";

            return await Task.FromResult<IEnumerable<Reminder>>(db.Query<Reminder>(query));
        }

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

        public long? GetTimestampOfNextReminder()
        {
            var db = DatabaseConnection.Instance.GetConnection();
            string query = @"SELECT MIN(Date) 
                             FROM Reminder 
                             WHERE Date > strftime('%s','now')";

            long nextReminderTimestamp = db.ExecuteScalar<long>(query);
            return (nextReminderTimestamp > 0) ? nextReminderTimestamp as long? : null;
        }
    }
}
