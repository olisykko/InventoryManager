using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using System.Data;
using TShockAPI.DB;

namespace InventoryManager
{
    public class DB
    {
        public static readonly IDbConnection db = new SqliteConnection("Data Sourece=" + Path.Combine("InventoryManager.sqlite"));
        public static void Setup()
        {
            SqlTableCreator sqlTable = new(db, new SqliteQueryCreator());
            sqlTable.EnsureTableStructure(new SqlTable("Inventories",
                new SqlColumn("Username", MySqlDbType.Text),
                new SqlColumn("UserID", MySqlDbType.Int32),
                new SqlColumn("Name", MySqlDbType.Text),
                new SqlColumn("Inventory", MySqlDbType.Text),
                new SqlColumn("Armor", MySqlDbType.Text),
                new SqlColumn("Dye", MySqlDbType.Text),
                new SqlColumn("HideVisibleAccessory", MySqlDbType.Text),
                new SqlColumn("MiscEquips", MySqlDbType.Text),
                new SqlColumn("MiscDyes", MySqlDbType.Text),
                new SqlColumn("Male", MySqlDbType.Text),
                new SqlColumn("Hair", MySqlDbType.Text),
                new SqlColumn("SkinVariant", MySqlDbType.Text),
                new SqlColumn("SkinColor", MySqlDbType.Text),
                new SqlColumn("EyeColor", MySqlDbType.Text),
                new SqlColumn("HairColor", MySqlDbType.Text),
                new SqlColumn("ShirtColor", MySqlDbType.Text),
                new SqlColumn("UnderShirtColor", MySqlDbType.Text),
                new SqlColumn("PantsColor", MySqlDbType.Text),
                new SqlColumn("ShoeColor", MySqlDbType.Text),
                new SqlColumn("HideMisc", MySqlDbType.Text),
                new SqlColumn("SuperCart", MySqlDbType.Text),
                new SqlColumn("IsPrivate", MySqlDbType.Int32)));
        }
    }
}
