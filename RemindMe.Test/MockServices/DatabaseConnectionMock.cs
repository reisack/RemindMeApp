using System;
using SQLite;
using RemindMe.Core.Models;
using RemindMe.Core.Interfaces;

namespace RemindMe.Test.MockServices
{
    public class DatabaseConnectionMock : IConnectionService
    {
        private static Lazy<DatabaseConnectionMock> _instance = new Lazy<DatabaseConnectionMock>(() => new DatabaseConnectionMock());
        private SQLiteConnection _connection;

        public DatabaseConnectionMock()
        {
            _connection = new SQLiteConnection("remindme_empty_mock.db");
            _connection.DropTable<Reminder>();
            _connection.CreateTable<Reminder>();
        }

        public SQLiteConnection GetConnection()
        {
            return _connection;
        }

        public static DatabaseConnectionMock Instance
        {
            get { return _instance.Value; }
        }
    }
}
