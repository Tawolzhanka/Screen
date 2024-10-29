using System.Data.SQLite;

namespace Screen.Repositories
{
    public interface IOrderDetailsRepository
    {
        void InsertOrUpdateOrderDetails(SQLiteConnection conn, int orderId, int productId, int quantity, decimal unitPrice);
    }
}
