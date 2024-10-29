using System;
using System.Data.SQLite;

public class UserRepository : IUserRepository
{
    public int GetOrInsertUser(SQLiteConnection conn, string fio, string email)
    {
        using (var cmd = new SQLiteCommand("SELECT user_id, password_hash FROM Users WHERE username = @Fio AND email = @Email", conn))
        {
            cmd.Parameters.AddWithValue("@Fio", fio);
            cmd.Parameters.AddWithValue("@Email", email);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    string passwordHash = reader.IsDBNull(1) ? null : reader.GetString(1);

                    if (string.IsNullOrEmpty(passwordHash))
                    {
                        Console.WriteLine($"Пароль у пользователя {fio} не установлен, задаем значение по умолчанию.");
                        UpdatePasswordHash(conn, userId, "password");
                    }

                    return userId;
                }
            }
        }

        using (var insertCmd = new SQLiteCommand("INSERT INTO Users (username, email, password_hash) VALUES (@Fio, @Email, @PasswordHash); SELECT last_insert_rowid();", conn))
        {
            insertCmd.Parameters.AddWithValue("@Fio", fio);
            insertCmd.Parameters.AddWithValue("@Email", email);
            insertCmd.Parameters.AddWithValue("@PasswordHash", "password");

            return Convert.ToInt32(insertCmd.ExecuteScalar());
        }
    }

    public void UpdatePasswordHash(SQLiteConnection conn, int userId, string newPasswordHash)
    {
        using (var cmd = new SQLiteCommand("UPDATE Users SET password_hash = @PasswordHash WHERE user_id = @UserId", conn))
        {
            cmd.Parameters.AddWithValue("@PasswordHash", newPasswordHash);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.ExecuteNonQuery();
        }
    }
}
