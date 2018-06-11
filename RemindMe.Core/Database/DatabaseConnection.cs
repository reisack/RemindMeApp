using RemindMe.Core.Models;
using SQLite;
using System;
using System.IO;

namespace RemindMe.Core.Database
{
    public class DatabaseConnection
    {
        private static Lazy<DatabaseConnection> _instance = new Lazy<DatabaseConnection>(() => new DatabaseConnection());
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

        public static DatabaseConnection Instance
        {
            get { return _instance.Value; }
        }
    }
}
