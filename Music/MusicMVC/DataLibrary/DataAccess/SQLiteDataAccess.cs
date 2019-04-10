using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace DataLibrary.DataAccess
{
  public static class SQLiteDataAccess
  {
    private const string CONNECTION_STRING = "Data Source=./MusicDB.sqlite; Version=3;";

    // abstract away database type
    private static IDbConnection GetConnection()
    {
      return (new SQLiteConnection(CONNECTION_STRING));
    }

    public static List<T> LoadData<T>(string sql)
    {
      using (IDbConnection connection = GetConnection())
      {
        var result = connection.Query<T>(sql);
        return (result.ToList());
      }
    }

    public static int SaveData<T>(string sql, T data)
    {
      using (IDbConnection connection = GetConnection())
      {
        int rowsAffected = connection.Execute(sql, data);
        return (rowsAffected);
      }
    }

    public static int InsertGetId<T>(string sql, T data)
    {
      using (IDbConnection connection = GetConnection())
      {
        int newId = connection.Query<int>(sql, data).Single();
        return (newId);
      }
    }
  }
}
