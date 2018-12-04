using System;
using SQLite;
using RemindMe.Core.Interfaces;

namespace RemindMe.Test.Mocks
{
    public class DatabaseConnectionMock : IConnectionService
    {
        private static Lazy<DatabaseConnectionMock> _instance = new Lazy<DatabaseConnectionMock>(() => new DatabaseConnectionMock());
        private SQLiteConnection _connection;

        public DatabaseConnectionMock()
        {
            // In-memory database : extremely fast
            _connection = new SQLiteConnection(":memory:");
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
