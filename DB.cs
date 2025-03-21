using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using System.Data;
using TShockAPI.DB;

namespace InventoryManager
{
    public class DB
    {
        public static readonly IDbConnection db = new SqliteConnection("Data Source=" + Path.Combine("tshock", "InventoryManager.sqlite"));
        public static void Setup()
        {
            SqlTableCreator sqlTable = new(db, new SqliteQueryCreator());
            sqlTable.EnsureTableStructure(new SqlTable("Inventories",
                new SqlColumn("Username", MySqlDbType.Text),
                new SqlColumn("Name", MySqlDbType.Text),
                new SqlColumn("Inventory", MySqlDbType.Text)));
        }
    }
}
