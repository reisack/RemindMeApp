using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindMe.Core.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly IConnectionService _connectionService;

        public ReminderRepository(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<int> AddOrUpdate(Reminder reminder)
        {
            return await Task.Run(() =>
            {
                var db = _connectionService.GetConnection();
                var existingReminder = db.Find<Reminder>(reminder.Id);
                if (existingReminder != null)
                {
                    return db.Update(reminder, typeof(Reminder));
                }
                else
                {
                    return db.Insert(reminder, typeof(Reminder));
                }
            });
        }

        public async Task<int> Delete(long id)
        {
            return await Task.Run(() =>
            {
                var db = _connectionService.GetConnection();
                return db.Delete<Reminder>(id);
            });
        }

        public async Task<int> DeletePast()
        {
            return await Task.Run(() =>
            {
                var db = _connectionService.GetConnection();
                string query = @"DELETE FROM Reminder WHERE Date < strftime('%s','now')";
                return db.Execute(query);
            });
        }

        public async Task<Reminder> Get(long id)
        {
            var db = _connectionService.GetConnection();

            return await Task.FromResult<Reminder>(db.Find<Reminder>(id));
        }

        public async Task<IEnumerable<Reminder>> GetAll()
        {
            var db = _connectionService.GetConnection();

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
            var db = _connectionService.GetConnection();

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

                var db = _connectionService.GetConnection();
                db.UpdateAll(reminders);
            });
        }

        public long? GetNextReminderTimestamp()
        {
            var db = _connectionService.GetConnection();
            string query = @"SELECT Id, Title, Comment, Date, AlreadyNotified 
                             FROM Reminder 
                             WHERE Date > strftime('%s','now') 
                             ORDER BY Date ASC
                             LIMIT 1";

            long nextReminderTimestamp = 0;
            List<Reminder> queryResult = db.Query<Reminder>(query);

            if (queryResult != null && queryResult.Count > 0)
            {
                Reminder reminder = queryResult[0];
                if (reminder != null && reminder.Date > 0)
                {
                    nextReminderTimestamp = reminder.Date;
                }
            }

            return (nextReminderTimestamp > 0) ? nextReminderTimestamp as long? : null;
        }
    }
}
