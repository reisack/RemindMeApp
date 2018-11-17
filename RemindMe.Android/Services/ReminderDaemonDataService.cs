using System.Collections.Generic;
using System.Threading.Tasks;
using RemindMe.Core.Models;
using RemindMe.Core.Repositories;

namespace RemindMe.Android.Services
{
    public class ReminderDaemonDataService
    {
        public ReminderRepository _reminderRepository;

        public ReminderDaemonDataService()
        {
            _reminderRepository = new ReminderRepository();
        }

        public async Task<IEnumerable<Reminder>> GetRemindersToNotify()
        {
            return await _reminderRepository.GetRemindersToNotify();
        }

        public async Task SetToNotified(IEnumerable<Reminder> reminders)
        {
            await _reminderRepository.SetToNotified(reminders);
        }

        public long? GetTimestampOfNextReminder()
        {
            return _reminderRepository.GetTimestampOfNextReminder();
        }
    }
}