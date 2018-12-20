using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using System.IO;

namespace Offline
{
  class MainClass
  {
    const string PROJECT_PATH = "/media/name/DataDrive/Coding Challenges/C#/CardMatch/";
    const string DB_PATH = PROJECT_PATH + "Data/DB/cardMatch.sqlite";
    const string MTG_JSON_PATH = PROJECT_PATH + "Data/MTGJSON/";
    const string IMAGE_PATH = PROJECT_PATH + "Public/card/image/";
    const string CONNECTION_STRING = "Data Source=" + DB_PATH;
    static readonly Dictionary<string, int> matchType = new Dictionary<string, int>
    {
      { "base", 1 },
      { "cover", 2 }
    };
    static readonly Dictionary<string, int> artFrame = new Dictionary<string, int>
    {
      { "2003", 1 },
      { "2015", 2 }
    };
    static readonly Dictionary<string, int> userType = new Dictionary<string, int>
    {
      { "admin", 1 },
      { "client", 2 }
    };
    static readonly Dictionary<string, int> encryption = new Dictionary<string, int>
    {
      { "plain", 1 }
    };

    static Dictionary<string, Action> actionDict = new Dictionary<string, Action> {
      { "help", new Action("list actions", ListActions) },
      { "quit", new Action("exits program", () => { }) },
      { "addSet", new Action("adds set to database", AddSet) },
      { "createTables", new Action("creates database tables", CreateTables)}
    };

    public static void Main(string[] args)
    {
      do
      {
        Console.WriteLine("Please specify action");
      } while (TakeAction(Console.ReadLine()));
    }

    static bool TakeAction(string action)
    {
      if ("quit" == action)
      {
        return (false);
      }
      else if (actionDict.ContainsKey(action))
      {
        actionDict[action].Run();
      }
      else
      {
        Console.WriteLine("action not understood. Type 'help' for list of actions");
      }
      return (true);
    }

    static void ListActions()
    {
      foreach (var actionPair in actionDict)
      {
        Console.WriteLine("{0}: {1}", actionPair.Key, actionPair.Value.GetDescription());
      }
    }

    static void CreateTables()
    {
      const string CREATE_IF = "CREATE TABLE IF NOT EXISTS ";
      const string NN_REF = " INTEGER NOT NULL REFERENCES ";
      string[] tableCommand = new string[] {
        CREATE_IF + "CardSet("
        + "cardSetId INTEGER PRIMARY KEY, "
        + "releaseDate TEXT NOT NULL, "
        + "cardSetName TEXT NOT NULL UNIQUE, "
        + "cardSetCode TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "MatchType("
        + "matchTypeId INTEGER PRIMARY KEY, "
        + "matchType TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Card("
        + "cardId INTEGER PRIMARY KEY, "
        + "matchType" + NN_REF + "MatchType(matchTypeId), "
        + "cardName TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "CardImage("
        + "cardImageId INTEGER PRIMARY KEY, "
        + "artFrame" + NN_REF + "ArtFrame(artFrameId), "
        + "card" + NN_REF + "Card(cardId), "
        + "isPotential INTEGER NOT NULL, "
        + "cardImagePath TEXT NOT NULL"
        + ")",
        CREATE_IF + "ArtFrame("
        + "artFrameId INTEGER PRIMARY KEY, "
        + "artFrame TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Release("
        + "releaseId INTEGER PRIMARY KEY, "
        + "cardSet" + NN_REF + "CardSet(cardSetId), "
        + "card" + NN_REF + "Card(cardId), "
        + "cardImage" + NN_REF + "CardImage(cardImageId), "
        + "inventory INTEGER NOT NULL, "
        + "cardNumber TEXT NOT NULL"
        + ")",
        CREATE_IF + "Match("
        + "matchId INTEGER PRIMARY KEY, "
        + "isMatch INTEGER NOT NULL, "
        + "base" + NN_REF + "CardImage(cardImageId), "
        + "cover" + NN_REF + "CardImage(cardImageId), "
        + "inventory INTEGER NOT NULL, "
        + "requestCount INTEGER NOT NULL, "
        + "UNIQUE (base, cover)"
        + ")",
        CREATE_IF + "Store("
        + "storeId INTEGER PRIMARY KEY, "
        + "storeName TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Price("
        + "priceId INTEGER PRIMARY KEY, "
        + "release" + NN_REF + "Release(releaseId), "
        + "store" + NN_REF + "Store(storeId), "
        + "date TEXT NOT NULL, "
        + "price INTEGER NOT NULL"
        + ")",
        CREATE_IF + "Encryption("
        + "encryptionId INTEGER PRIMARY KEY, "
        + "encryption TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "UserType("
        + "userTypeId INTEGER PRIMARY KEY, "
        + "userType TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "User("
        + "userId INTEGER PRIMARY KEY, "
        + "userType" + NN_REF + "UserType(userTypeId), "
        + "encryption" + NN_REF + "Encryption(encryptionId), "
        + "salt TEXT NOT NULL, "
        + "pass TEXT NOT NULL, "
        + "userName TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Session("
        + "cookie TEXT PRIMARY KEY, "
        + "user" + NN_REF + "User(userId) UNIQUE, "
        + "expires INTEGER NOT NULL"
        + ")"
      };
      string[] tableFill =
      {
        "INSERT OR IGNORE INTO MatchType ( matchTypeId, matchType ) "
        + "VALUES ( " + matchType["base"] + ", 'base' )",
        "INSERT OR IGNORE INTO MatchType ( matchTypeId, matchType ) "
        + "VALUES ( " + matchType["cover"] + ", 'cover' )",
        "INSERT OR IGNORE INTO ArtFrame ( artFrameId, artFrame ) "
        + "VALUES ( " + artFrame["2003"] + ", '2003' )",
        "INSERT OR IGNORE INTO ArtFrame ( artFrameId, artFrame ) "
        + "VALUES ( " + artFrame["2015"] + ", '2015' )",
        "INSERT OR IGNORE INTO UserType ( userTypeId, userType ) "
        + "VALUES ( " + userType["admin"] + ", 'admin' )",
        "INSERT OR IGNORE INTO UserType ( userTypeId, userType ) "
        + "VALUES ( " + userType["client"] + ", 'client' )",
        "INSERT OR IGNORE INTO Encryption ( encryptionId, encryption ) "
        + "VALUES ( " + encryption["plain"] + ", 'plain' )",
      };
      using (SqliteConnection connection = new SqliteConnection(CONNECTION_STRING))
      {
        connection.Open();
        using (SqliteCommand command = new SqliteCommand(connection))
        {
          foreach (string table in tableCommand)
          {
            Console.WriteLine(table);
            command.CommandText = table;
            command.ExecuteNonQuery();
          }
          foreach (string fill in tableFill)
          {
            Console.WriteLine(fill);
            command.CommandText = fill;
            command.ExecuteNonQuery();
          }
        }
        connection.Close();
      }
      Console.WriteLine("Completed");
    }

    static void AddSet()
    {
      Console.WriteLine("Enter set code");
      string setCode = Console.ReadLine().ToUpper();

      if (!SetFileExists(setCode))
      {
        Console.WriteLine("Set JSON not found");
      }
      else
      {
        Console.WriteLine("Adding set");
        string json = File.ReadAllText(GetMtGSetFilePath(setCode));
        MtGSet mySet = JsonConvert.DeserializeObject<MtGSet>(json);
        mySet.FilterValid();
        InsertSet(mySet);
      }
    }

    static string GetMtGSetFilePath(string setCode)
    {
      return (MTG_JSON_PATH + setCode + ".json");
    }

    static string GetImagePath(string setCode, string cardName)
    {
      string path = IMAGE_PATH + setCode + "/" + cardName + ".xlhq.jpg";
      return (File.Exists(path) ? path : "");
    }

    static bool SetFileExists(string setCode)
    {
      return (File.Exists(GetMtGSetFilePath(setCode)));
    }

    static void InsertSet(MtGSet set)
    {
      // connect to db
      using (SqliteConnection connection = new SqliteConnection(CONNECTION_STRING))
      {
        connection.Open();

        // rollback if anything fails
        using (SqliteTransaction transaction = connection.BeginTransaction())
        {
          using (SqliteCommand command = new SqliteCommand(connection))
          {
            command.Transaction = transaction;

            // create set
            Console.WriteLine("creating set");
            command.CommandText = "INSERT OR IGNORE INTO CardSet "
            + "( releaseDate, cardSetName, cardSetCode ) VALUES "
            + "( @releaseDate, @cardSetName, @cardSetCode )";
            command.Prepare();
            command.Parameters.AddWithValue("@releaseDate", set.GetDate());
            command.Parameters.AddWithValue("@cardSetName", set.GetName());
            command.Parameters.AddWithValue("@cardSetCode", set.GetCode());

            // if set was new, fill cards
            if (1 == command.ExecuteNonQuery())
            {

              // get card id
              command.CommandText = "SELECT cardSetId FROM CardSet "
                + "WHERE cardSetCode = @cardSetCode";
              command.Prepare();
              command.Parameters.AddWithValue("@cardSetCode", set.GetCode());
              int cardId = Convert.ToInt32(command.ExecuteScalar());

              Console.WriteLine("creating cards");
              foreach (Card card in set.GetCards())
              {
                // create card
                command.CommandText = "INSERT OR IGNORE INTO Card "
                + "( matchType, cardName ) VALUES "
                + "( @matchType, @cardName )";
                command.Prepare();
                command.Parameters.AddWithValue("@matchType",
                  card.IsBase() ? matchType["base"] : matchType["cover"]);
                command.Parameters.AddWithValue("@cardName", card.GetName());
                bool cardCreated = 1 == command.ExecuteNonQuery();

                // get card id
                command.CommandText = "SELECT cardId FROM Card "
                  + "WHERE cardName = @cardName";
                command.Prepare();
                command.Parameters.AddWithValue("@cardName", card.GetName());
                int setId = Convert.ToInt32(command.ExecuteScalar());

                // if card was new, fill image
                int imageId = -1; // default to invalid
                if (cardCreated)
                {
                  // create image
                  command.CommandText = "INSERT INTO CardImage "
                  + "( artFrame, card, isPotential, cardImagePath ) VALUES "
                  + "( @artFrame, @card, @isPotential, @cardImagePath )";
                  command.Prepare();
                  command.Parameters.AddWithValue(
                    "@artFrame", artFrame[card.GetFrame()]);
                  command.Parameters.AddWithValue("@card", cardId);
                  command.Parameters.AddWithValue("@isPotential", -1);
                  command.Parameters.AddWithValue("@cardImagePath",
                    GetImagePath(set.GetCode(), card.GetName()));
                  command.ExecuteNonQuery();

                  // get image id
                  command.CommandText = "SELECT cardImageId FROM CardImage "
                  + "WHERE card = @card";
                  command.Prepare();
                  command.Parameters.AddWithValue("@card", cardId);
                  imageId = Convert.ToInt32(command.ExecuteScalar());
                }

                // fill release
                command.CommandText = "INSERT INTO Release "
                + "( cardSet, card, cardImage, inventory, cardNumber ) VALUES "
                + "( @cardSet, @card, @cardImage, @inventory, @cardNumber )";
                command.Prepare();
                command.Parameters.AddWithValue("@cardSet", setId);
                command.Parameters.AddWithValue("@card", cardId);
                command.Parameters.AddWithValue("@cardImage", imageId);
                command.Parameters.AddWithValue("@inventory", 0);
                command.Parameters.AddWithValue("@cardNumber", card.GetNumber());
                command.ExecuteNonQuery();
              }           
            }

          }
          transaction.Commit();
        }

        connection.Close();
      }
      Console.WriteLine("Completed");
      // add set
      // create new card
      // add card release
      ;
    }
  }
}
