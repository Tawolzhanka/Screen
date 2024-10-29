using System;
using System.Data.SQLite;

namespace Screen.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public int GetOrInsertProduct(SQLiteConnection conn, string name, decimal price)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT product_id FROM Products WHERE name = @Name", conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Products (name, price) VALUES (@Name, @Price); SELECT last_insert_rowid();", conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
