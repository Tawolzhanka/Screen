using System;
using System.Data.SQLite;

namespace Screen.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        public void InsertOrUpdateOrderDetails(SQLiteConnection conn, int orderNo, int productId, int quantity, decimal price)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(
                "INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price) " +
                "VALUES (@OrderNo, @ProductId, @Quantity, @UnitPrice) " +
                "ON CONFLICT(order_id, product_id) DO UPDATE SET quantity = @Quantity, unit_price = @UnitPrice;", conn))
            {
                cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", price);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
