using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindMe.Core.Services
{
    public class ReminderDataService : IReminderDataService
    {
        private readonly IReminderRepository _reminderRepository;

        public ReminderDataService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task AddOrUpdate(Reminder reminder)
        {
            reminder.AlreadyNotified = 0;
            await _reminderRepository.AddOrUpdate(reminder);
        }

        public async Task Delete(long id)
        {
            await _reminderRepository.Delete(id);
        }

        public async Task DeletePast()
        {
            await _reminderRepository.DeletePast();
        }

        public async Task<Reminder> Get(long id)
        {
            return await _reminderRepository.Get(id);
        }

        public async Task<IEnumerable<Reminder>> GetAll()
        {
            return await _reminderRepository.GetAll();
        }

        public async Task<IEnumerable<Reminder>> GetRemindersToNotify()
        {
            return await _reminderRepository.GetRemindersToNotify();
        }

        public async Task SetToNotified(IEnumerable<Reminder> reminders)
        {
            await _reminderRepository.SetToNotified(reminders);
        }
    }
}
