using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemindMe.Core.Interfaces
{
    public interface IConnectionService
    {
        SQLiteConnection GetConnection();
    }
}
