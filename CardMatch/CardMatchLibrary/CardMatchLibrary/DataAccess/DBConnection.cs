using System;
using Mono.Data.Sqlite;
using System.Data;

namespace CardMatchLibrary.DataAccess
{
  public class DBConnection
  {
    private SqliteConnection connection { get; set; } = null;
    private SqliteTransaction transaction { get; set; } = null;
    private SqliteCommand command { get; set; } = null;

    public DBConnection()
    {
      const string CONNECTION_STRING =
        "Data Source=./Data/cardMatch.sqlite;Version=3;";
      connection = new SqliteConnection(CONNECTION_STRING);
    }

    ~DBConnection()
    {
      CloseCommand();
      CloseTransaction();
      CloseConnection();
      if (ConnectionExists())
      {
        connection.Dispose();
      }
    }

    private bool ConnectionExists()
    {
      return (null != connection);
    }

    private bool TransactionExists()
    {
      return (null != transaction);
    }

    private bool CommandExists()
    {
      return (null != command);
    }

    public void OpenConnection()
    {
      if (ConnectionExists())
      {
        connection.Open();
      }
    }

    public void CloseConnection()
    {
      if (ConnectionExists())
      {
        connection.Close();
      }
    }

    public void OpenTransaction()
    {
      if (!TransactionExists() && ConnectionExists())
      {
        transaction = connection.BeginTransaction();
      }
    }

    public void CloseTransaction()
    {
      if (TransactionExists())
      {
        transaction.Dispose();
        transaction = null;
      }
    }

    public void CommitTransaction()
    {
      if (TransactionExists())
      {
        transaction.Commit();
        CloseTransaction();
      }
    }

    public void OpenCommand()
    {
      if (!CommandExists() && ConnectionExists())
      {
        command = new SqliteCommand(connection);
      }
    }

    public void CloseCommand()
    {
      if (CommandExists())
      {
        command.Dispose();
        command = null;
      }
    }

    public void CommandSetText(string commandText)
    {
      if (CommandExists())
      {
        command.CommandText = commandText;
      }
    }

    public void CommandSetValue<T>(string parameterName, T value)
    {
      if (CommandExists())
      {
        command.Parameters.AddWithValue(parameterName, value);
      }
    }

    public void CommandPrepare()
    {
      if (CommandExists())
      {
        command.Prepare();
      }
    }

    public int CommandExecuteNonQuery()
    {
      int changeCount = -1;
      if (CommandExists())
      {
        changeCount = command.ExecuteNonQuery();
        CloseCommand();
      }
      return (changeCount);
    }

    public Object CommandExecuteScalar()
    {
      Object value = null;
      if (CommandExists())
      {
        value = command.ExecuteScalar();
        CloseCommand();
      }
      return (value);
    }

    public int CommandInsertGetId()
    {
      long? id = null;
      if (CommandExists())
      {
        CommandSetText(command.CommandText + "; SELECT last_insert_rowid()");
        id = (long)CommandExecuteScalar();
      }
      return (int)(id ?? -1);
    }

    public void CommandExecuteDataTable(DataTable dataTable)
    {
      if (ConnectionExists() && CommandExists())
      {
        using (SqliteDataAdapter adapter = new SqliteDataAdapter())
        {
          adapter.SelectCommand = new SqliteCommand(command.CommandText, connection);
          foreach (SqliteParameter parameter in command.Parameters)
          {
            adapter.SelectCommand.Parameters.Add(parameter);
          }
          adapter.Fill(dataTable);
          CloseCommand();
        }
      }
    }

    public int GetLastId()
    {
      long? lastId = null;
      if (!CommandExists() && ConnectionExists())
      {
        OpenCommand();
        CommandSetText("SELECT last_insert_rowid()");
        lastId = (long)CommandExecuteScalar();
        CloseCommand();
      }
      return (int)(lastId ?? -1);
    }
  }
}
