using System.Data;
using System.Data.SQLite;

namespace DataLibrary.DataAccess
{
  public static class SQLiteDataAccess
  {
    private const string CONNECTION_STRING = "Data Source=./Data/GroceryDB.sqlite; Version=3;";

    public static IDbConnection GetConnection()
    {
      return (new SQLiteConnection(CONNECTION_STRING));
    }
  }
}
