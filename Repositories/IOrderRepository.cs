using System;
using System.Data.SQLite;

namespace Screen.Repositories
{
    public interface IOrderRepository
    {
        void InsertOrUpdateOrder(SQLiteConnection conn, int userId, DateTime regDate, decimal totalAmount, int orderNo); 
        bool IsOrderAlreadyProcessed(SQLiteConnection conn, int orderNo);
        void MarkOrderAsProcessed(SQLiteConnection conn, int orderId);
    }
}
