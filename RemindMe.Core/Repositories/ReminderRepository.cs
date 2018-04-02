using RemindMe.Core.Database;
using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<Reminder> Get(long id)
        {
            var db = DatabaseConnection.Instance.GetConnection();

            return await Task.FromResult<Reminder>(db.Find<Reminder>(id));
        }

        public async Task<IEnumerable<Reminder>> GetAll()
        {
            var db = DatabaseConnection.Instance.GetConnection();

            string query = @"SELECT Id, Title, Comment, Date 
                             FROM Reminder 
                             ORDER BY Date ASC, Title ASC";

            return await Task.FromResult<IEnumerable<Reminder>>(db.Query<Reminder>(query));
        }
    }
}
