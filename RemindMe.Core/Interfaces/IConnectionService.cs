using SQLite;

namespace RemindMe.Core.Interfaces
{
    public interface IConnectionService
    {
        SQLiteConnection GetConnection();
    }
}
