using System;
using SQLite;
using RemindMe.Core.Interfaces;

namespace RemindMe.Test.Fakes
{
    public class DatabaseConnectionFake : IConnectionService
    {
        private static Lazy<DatabaseConnectionFake> _singletonInstance = new Lazy<DatabaseConnectionFake>(() => new DatabaseConnectionFake());
        private SQLiteConnection _connection;

        public DatabaseConnectionFake()
        {
            // In-memory database : extremely fast
            _connection = new SQLiteConnection(":memory:");
        }

        public SQLiteConnection GetConnection()
        {
            return _connection;
        }

        public static DatabaseConnectionFake SingletonInstance
        {
            get { return _singletonInstance.Value; }
        }
    }
}
