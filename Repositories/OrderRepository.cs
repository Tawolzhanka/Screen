using System;
using System.Data.SQLite;

namespace Screen.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void InsertOrUpdateOrder(SQLiteConnection conn, int userId, DateTime regDate, decimal totalAmount, int orderNo)
        { 
            using (var cmd = new SQLiteCommand("SELECT user_id, order_date, total_amount FROM Orders WHERE order_id = @OrderNo", conn))  //проверка на существование заказа
            {
                cmd.Parameters.AddWithValue("@OrderNo", orderNo);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // если заказ существует, то проверка, нужно ли обновлять данные
                    {
                        int existingUserId = reader.GetInt32(0); 
                        DateTime existingRegDate = reader.GetDateTime(1);
                        decimal existingTotalAmount = reader.GetDecimal(2);

                        
                        if (existingUserId != userId || existingRegDate != regDate || existingTotalAmount != totalAmount)  // обновление записи, если данные различаются
                        {
                            using (var updateCmd = new SQLiteCommand("UPDATE Orders SET user_id = @UserId, order_date = @OrderDate, total_amount = @TotalAmount WHERE order_id = @OrderNo", conn))
                            {
                                updateCmd.Parameters.AddWithValue("@UserId", userId);
                                updateCmd.Parameters.AddWithValue("@OrderDate", regDate);
                                updateCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                                updateCmd.Parameters.AddWithValue("@OrderNo", orderNo);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else // добавление нового заказа, если данные отсутствуют
                    {
                       
                        using (var insertCmd = new SQLiteCommand( 
                            "INSERT INTO Orders (order_id, user_id, order_date, total_amount, is_processed) VALUES (@OrderNo, @UserId, @OrderDate, @TotalAmount, 0)", conn))
                        {
                            insertCmd.Parameters.AddWithValue("@OrderNo", orderNo);
                            insertCmd.Parameters.AddWithValue("@UserId", userId);
                            insertCmd.Parameters.AddWithValue("@OrderDate", regDate);
                            insertCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public bool IsOrderAlreadyProcessed(SQLiteConnection conn, int orderNo)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Orders WHERE order_id = @OrderNo AND is_processed = 1", conn))
            {
                cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void MarkOrderAsProcessed(SQLiteConnection conn, int orderId)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("UPDATE Orders SET is_processed = 1 WHERE order_id = @OrderId", conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
