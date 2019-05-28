using System.Data;
using Mono.Data.Sqlite;

namespace DataLibrary.DataAccess
{
  public static class SQLiteDataAccess
  {
    private const string CONNECTION_STRING = "Data Source=./../Data/GroceryDB.sqlite; Version=3;";

    public static IDbConnection GetConnection() => new SqliteConnection(CONNECTION_STRING);
  }
}
