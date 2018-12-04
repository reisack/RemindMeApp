using RemindMe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindMe.Core.Interfaces
{
    public interface IReminderRepository
    {
        Task<Reminder> Get(long id);
        Task<IEnumerable<Reminder>> GetAll();
        Task<int> AddOrUpdate(Reminder reminder);
        Task<int> Delete(long id);
        Task<int> DeletePast();
        Task<IEnumerable<Reminder>> GetRemindersToNotify();
        Task SetToNotified(IEnumerable<Reminder> reminders);
        long? GetNextReminderTimestamp();
    }
}
