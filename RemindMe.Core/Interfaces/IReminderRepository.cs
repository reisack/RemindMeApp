﻿using RemindMe.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Core.Interfaces
{
    public interface IReminderRepository
    {
        Task<Reminder> Get(long id);
        Task<IEnumerable<Reminder>> GetAll();
        Task AddOrUpdate(Reminder reminder);
        Task Delete(long id);
    }
}
