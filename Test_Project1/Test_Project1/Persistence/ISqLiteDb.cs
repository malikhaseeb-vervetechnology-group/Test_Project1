using SQLite;

namespace Test_Project1.Persistence
{
    public interface ISqLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
