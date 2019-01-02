using System;
using CardMatchLibrary.Models;
using System.Data;

namespace CardMatchLibrary.DataAccess
{
  public static class DataConnector
  {
    public static void CreateCardSet(CardSetModel cardSet)
    {
      // only create if not already in database
      if (null != cardSet.code && -1 == GetCardSetId(cardSet.code))
      {
        // initialize
        DBConnection conn = new DBConnection();
        conn.OpenConnection();
        conn.OpenTransaction();

        // create the set
        conn.OpenCommand();
        conn.CommandSetText(
          "INSERT INTO CardSet ( code, releaseDate, name ) "
          + "VALUES ( @code, @releaseDate, @name )");
        conn.CommandSetValue("@code", cardSet.code);
        conn.CommandSetValue("@releaseDate", cardSet.releaseDate);
        conn.CommandSetValue("@name", cardSet.name);
        cardSet.id = conn.CommandInsertGetId();
        conn.CloseCommand();

        // create releases
        foreach (ReleaseModel release in cardSet.releases)
        {
          CreateRelease(conn, release, cardSet);
        }

        // clean up
        conn.CommitTransaction();
        conn.CloseTransaction();
        conn.CloseConnection();
      }
    }

    private static void CreateRelease(DBConnection conn, ReleaseModel release, CardSetModel cardSet)
    {
      // create the card
      CreateCard(conn, release.card);

      // create the image
      CreateImage(conn, release.image);

      // create the release
      conn.OpenCommand();
      conn.CommandSetText(
        "INSERT INTO Release ( cardSet, image, card, cardNumber ) "
        + "VALUES ( @cardSet, @image, @card, @cardNumber )");
      conn.CommandSetValue("@cardSet", cardSet.id);
      conn.CommandSetValue("@image", release.image.id);
      conn.CommandSetValue("@card", release.card.id);
      conn.CommandSetValue("@cardNumber", release.cardNumber);
      release.id = conn.CommandInsertGetId();
      conn.CloseCommand();
    }

    private static void CreateCard(DBConnection conn, CardModel card)
    {
      // only create if not already in database
      card.id = GetCardId(card.name, conn);
      if (-1 == card.id)
      {
        // create the release
        conn.OpenCommand();
        conn.CommandSetText(
          "INSERT INTO Card ( isBase, name ) "
          + "VALUES ( @isBase, @name )");
        conn.CommandSetValue("@isBase", card.isBase);
        conn.CommandSetValue("@name", card.name);
        card.id = conn.CommandInsertGetId();
        conn.CloseCommand();
      }
    }

    private static void CreateImage(DBConnection conn, ImageModel image)
    {
      // create the release
      conn.OpenCommand();
      conn.CommandSetText(
        "INSERT INTO Image ( frame, matchStatus, hasCutout, canonicalImage, fileName, cardSetCode ) "
        + "VALUES ( @frame, @matchStatus, @hasCutout, @canonicalImage, @fileName, @cardSetCode )");
      conn.CommandSetValue("@frame", image.frame.id);
      conn.CommandSetValue("@matchStatus", image.matchStatus);
      conn.CommandSetValue("@hasCutout", image.hasCutout);
      conn.CommandSetValue("@canonicalImage", image.canonicalImageId);
      conn.CommandSetValue("@fileName", image.fileName);
      conn.CommandSetValue("@cardSetCode", image.cardSetCode);
      image.id = conn.CommandInsertGetId();
      conn.CloseCommand();
    }

    public static void CreateFrame(FrameModel frame)
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "INSERT OR IGNORE INTO Frame ( frame, yOffset ) "
        + "VALUES ( @frame, @yOffset )");
      conn.CommandSetValue("@frame", frame.frame);
      conn.CommandSetValue("@yOffset", frame.yOffset);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static void CreateMatch(MatchModel match)
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "INSERT OR IGNORE INTO Match ( baseImage, coverImage, judgment ) "
        + "VALUES ( @baseImage, @coverImage, @judgment )");
      conn.CommandSetValue("@baseImage", match.baseImage.id);
      conn.CommandSetValue("@coverImage", match.coverImage.id);
      conn.CommandSetValue("@judgment", match.judgment);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static FrameModel GetFrameModel(string frame)
    {
      var dataTable = new DataTable();
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "SELECT id, yOffset FROM Frame WHERE frame = @frame");
      conn.CommandSetValue("@frame", frame);
      conn.CommandExecuteDataTable(dataTable);
      conn.CloseCommand();
      conn.CloseConnection();

      if (1 != dataTable.Rows.Count)
      {
        return (new FrameModel(-1, "", 0));
      }
      return (new FrameModel(
        (int)(Int64)dataTable.Rows[0]["id"],
        frame,
        (int)(Int64)dataTable.Rows[0]["yOffset"]
      ));
    }

    private static int GetId(string commandText, string identifier, string value, DBConnection conn = null)
    {
      int id = -1;
      bool newConnection = null == conn;
      if (newConnection)
      {
        conn = new DBConnection();
        conn.OpenConnection();
      }
      conn.OpenCommand();
      conn.CommandSetText(commandText);
      conn.CommandSetValue(identifier, value);
      Object test = conn.CommandExecuteScalar();
      if (null != test)
      {
        id = (int)(long)test;
      }
      conn.CloseCommand();
      if (newConnection)
      {
        conn.CloseConnection();
      }
      return (id);
    }

    public static int GetCardSetId(string cardSetCode, DBConnection conn = null)
    {
      return (GetId(
        "SELECT id FROM CardSet WHERE code = @code",
        "@code",
        cardSetCode,
        conn
      ));
    }

    public static int GetCardId(string cardName, DBConnection conn = null)
    {
      return (GetId(
        "SELECT id FROM Card WHERE name = @name",
        "@name",
        cardName,
        conn
      ));
    }

    public static void CreateTables()
    {
      const string CREATE_IF = "CREATE TABLE IF NOT EXISTS ";
      const string PRIMARY_KEY = " INTEGER NOT NULL PRIMARY KEY UNIQUE, ";
      const string NN_REF = " INTEGER NOT NULL REFERENCES ";
      //const string INDEX_IF = "CREATE INDEX IF NOT EXISTS ";
      string[] tableCommand = new string[]
      {
        CREATE_IF + "CardSet("
        + "id" + PRIMARY_KEY
        + "code TEXT NOT NULL UNIQUE, "
        + "releaseDate TEXT NOT NULL, "
        + "name TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Release("
        + "id" + PRIMARY_KEY
        + "cardSet" + NN_REF + "CardSet(id), "
        + "card" + NN_REF + "Card(id), "
        + "image" + NN_REF + "Image(id), "
        + "cardNumber TEXT NOT NULL"
        + ")",
        CREATE_IF + "Card("
        + "id" + PRIMARY_KEY
        + "isBase INTEGER NOT NULL, "
        + "name TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Image("
        + "id" + PRIMARY_KEY
        + "frame" + NN_REF + "Frame(id), "
        + "canonicalImage" + NN_REF + "Image(id), "
        + "hasCutout INTEGER NOT NULL, "
        + "matchStatus TEXT NOT NULL, "
        + "cardSetCode TEXT NOT NULL, "
        + "fileName TEXT NOT NULL"
        + ")",
        CREATE_IF + "Frame("
        + "id" + PRIMARY_KEY
        + "yOffset INTEGER NOT NULL, "
        + "frame TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Match("
        + "id" + PRIMARY_KEY
        + "judgment TEXT NOT NULL, "
        + "baseImage" + NN_REF + "Image(id), "
        + "coverImage" + NN_REF + "Image(id), "
        + "UNIQUE (baseImage, coverImage)"
        + ")"
      };

      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      foreach (string command in tableCommand)
      {
        conn.OpenCommand();
        conn.CommandSetText(command);
        conn.CommandExecuteNonQuery();
        conn.CloseCommand();
      }
      conn.CloseConnection();

      CreateFrame(new FrameModel("2003"));
      CreateFrame(new FrameModel("2015"));
    }
  }
}
