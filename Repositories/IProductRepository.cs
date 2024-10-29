using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen.Repositories
{
    public interface IProductRepository
    {
        int GetOrInsertProduct(SQLiteConnection conn, string name, decimal price);
    }

}
