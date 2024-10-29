using System.Data.SQLite;

public interface IUserRepository
{
    int GetOrInsertUser(SQLiteConnection conn, string fio, string email);
}
