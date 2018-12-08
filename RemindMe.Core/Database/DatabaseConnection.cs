using RemindMe.Core.Interfaces;
using RemindMe.Core.Models;
using SQLite;
using System;
using System.IO;

namespace RemindMe.Core.Database
{
    public class DatabaseConnection : IConnectionService
    {
        private SQLiteConnection _connection;

        public DatabaseConnection()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            path = Path.Combine(path, "remindme.db");
            _connection = new SQLiteConnection(path);
            _connection.CreateTable<Reminder>();
        }

        public SQLiteConnection GetConnection()
        {
            return _connection;
        }
    }
}
