using System;
using CardMatchLibrary.Models;
using System.Data;
using System.Collections.Generic;
using Mono.Data.Sqlite;

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

        // update canonicalImage
        DetectCanonical();
      }
    }

    private static void CreateRelease(DBConnection conn, ReleaseModel release, CardSetModel cardSet)
    {
      // create the card
      CreateCard(conn, release.card);

      // create the release
      release.cardSetId = cardSet.id;
      release.cardSetCode = cardSet.code;
      release.VerifyImageFile();
      conn.OpenCommand();
      conn.CommandSetText(
        "INSERT INTO Release "
        + "( canonicalImage, cardSet, card, cardNumber, frame, "
        + "matchStatus, hasCutout, cardSetCode, imageFile )"
        + " VALUES "
        + "( @canonicalImage, @cardSet, @card, @cardNumber, @frame, "
        + "@matchStatus, @hasCutout, @cardSetCode, @imageFile )");
      conn.CommandSetValue("@canonicalImage", release.canonicalImageId);
      conn.CommandSetValue("@cardSet", release.cardSetId);
      conn.CommandSetValue("@card", release.card.id);
      conn.CommandSetValue("@cardNumber", release.cardNumber);
      conn.CommandSetValue("@frame", release.frame);
      conn.CommandSetValue("@matchStatus", release.matchStatus);
      conn.CommandSetValue("@hasCutout", release.hasCutout);
      conn.CommandSetValue("@cardSetCode", release.cardSetCode);
      conn.CommandSetValue("@imageFile", release.imageFile);

      release.id = conn.CommandInsertGetId();
      conn.CloseCommand();
    }

    private static void CreateCard(DBConnection conn, CardModel card)
    {
      // only create if not already in database
      card.id = GetCardId(card.name, conn);
      if (-1 == card.id)
      {
        // create the card
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

    public static ReleaseModel GetReleaseModel(int id)
    {
      var dataTable = new DataTable();
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "SELECT id, cardNumber, cardSetCode, imageFile, canonicalImage, "
        + "frame, matchStatus, hasCutout "
        + "FROM Release WHERE id = @id"
      );
      conn.CommandSetValue("@id", id);
      conn.CommandExecuteDataTable(dataTable);
      conn.CloseCommand();
      conn.CloseConnection();

      ReleaseModel release = new ReleaseModel();
      if (1 == dataTable.Rows.Count)
      {
        release.id = Convert.ToInt32(dataTable.Rows[0]["id"]);
        release.cardNumber = (string)dataTable.Rows[0]["cardNumber"];
        release.cardSetCode = (string)dataTable.Rows[0]["cardSetCode"];
        release.imageFile = (string)dataTable.Rows[0]["imageFile"];
        release.canonicalImageId = Convert.ToInt32(dataTable.Rows[0]["canonicalImage"]);
        release.frame = (string)dataTable.Rows[0]["frame"];
        release.matchStatus = (ReleaseModel.MatchStatus)Convert.ToInt32(dataTable.Rows[0]["matchStatus"]);
        release.hasCutout = Convert.ToBoolean(dataTable.Rows[0]["hasCutout"]);
      }
      return (release);
    }

    public static ReleaseModel GetReleaseNeedImage()
    {
      var dataTable = new DataTable();
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "SELECT Release.*, Card.name FROM Release "
        + "JOIN Card ON Release.card = Card.id "
        + "WHERE imageFile = '' LIMIT 1");
      conn.CommandExecuteDataTable(dataTable);
      conn.CloseCommand();
      conn.CloseConnection();

      ReleaseModel release = new ReleaseModel();
      release.card = new CardModel();

      if (1 == dataTable.Rows.Count)
      { 
        release.id = (int)(Int64)dataTable.Rows[0]["id"];
        release.cardNumber = (string)dataTable.Rows[0]["cardNumber"];
        release.cardSetCode = (string)dataTable.Rows[0]["cardSetCode"];
        release.card = new CardModel();
        release.card.name = (string)dataTable.Rows[0]["name"];
      }
      return (release);
    }

    public static void ReleaseAssignImage(ReleaseModel release)
    {
      // ensure file exists
      release.VerifyImageFile();

      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "UPDATE Release SET imageFile = @imageFile "
        + "WHERE cardSetCode = @cardSetCode "
        + "AND cardNumber = @cardNumber");
      conn.CommandSetValue("@imageFile", release.imageFile);
      conn.CommandSetValue("@cardSetCode", release.cardSetCode);
      conn.CommandSetValue("@cardNumber", release.cardNumber);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static void DetectCanonical()
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      // where no releases of a card+frame have defined canonical
      conn.CommandSetText(
        "UPDATE Release Set canonicalImage = Release.id " +
        "WHERE Release.id IN " +
        "(" +
          "SELECT Release.id FROM " +
          "(" +
            "SELECT Release.card, Release.frame, " +
              "COUNT(DISTINCT Release.canonicalImage) AS canonicalCount " +
            "FROM Release JOIN Card ON Release.card = Card.id " +
            "GROUP BY Release.card, Release.frame " +
          ") AS DefaultCanonical " +
          "JOIN Release ON Release.card = DefaultCanonical.card " +
            "AND Release.frame = DefaultCanonical.frame " +
          "WHERE canonicalCount = 1 " +
            "AND canonicalImage = -1 " +
            "AND imageFile != '' " +
          "GROUP BY Release.card, Release.frame " +
        ")"
      );
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static List<ReleaseModel> GetReleaseNeedCanonical()
    {
      var dataTable = new DataTable();
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      // get one unset canonical and all set canonical for card+frame
      conn.CommandSetText(
        "SELECT Release.id, Release.canonicalImage, Release.cardSetCode, " +
          "Release.imageFile, Release.frame " +
        "FROM Release " +
        "JOIN " +
        "(" +
          "SELECT Release.card, Release.frame " +
          "FROM Release " +
          "WHERE Release.canonicalImage = -1 " +
          "AND Release.imageFile != '' " +
          "LIMIT 1" +
        ") AS r ON Release.card = r.card AND Release.frame = r.frame " +
        "WHERE Release.imageFile != '' " +
        "GROUP BY Release.canonicalImage"
      );
      conn.CommandExecuteDataTable(dataTable);
      conn.CloseCommand();
      conn.CloseConnection();

      List<ReleaseModel> releases = new List<ReleaseModel>();
      foreach (DataRow row in dataTable.Rows)
      {
        ReleaseModel release = new ReleaseModel();
        release.id = (int)(long)row["id"];
        release.canonicalImageId = (int)(long)row["canonicalImage"];
        release.cardSetCode = (string)row["cardSetCode"];
        release.imageFile = (string)row["imageFile"];
        release.frame = (string)row["frame"];
        releases.Add(release);
      }

      return (releases);
    }

    public static void ReleaseAssignCanonical(ReleaseModel release)
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "UPDATE Release Set canonicalImage = @canonicalImage WHERE id = @id"
      );
      conn.CommandSetValue("@canonicalImage", release.canonicalImageId);
      conn.CommandSetValue("@id", release.id);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static ReleaseModel GetReleaseNeedJudgeMatchability()
    {
      var dataTable = new DataTable();
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      // is canonical image, matchStatus unchecked
      conn.CommandSetText(
        "SELECT id, cardSetCode, imageFile, frame FROM Release "
        + "WHERE canonicalImage == id AND matchStatus = @matchStatus "
        + "LIMIT 1"
      );
      conn.CommandSetValue("@matchStatus", ReleaseModel.MatchStatus.Unchecked);
      conn.CommandExecuteDataTable(dataTable);
      conn.CloseCommand();
      conn.CloseConnection();

      ReleaseModel release = new ReleaseModel();
      if (1 == dataTable.Rows.Count)
      {
        release.id = (int)(Int64)dataTable.Rows[0]["id"];
        release.cardSetCode = (string)dataTable.Rows[0]["cardSetCode"];
        release.imageFile = (string)dataTable.Rows[0]["imageFile"];
        release.frame = (string)dataTable.Rows[0]["frame"];
        release.matchStatus = ReleaseModel.MatchStatus.Unchecked;
      }
      return (release);
    }

    public static void ReleaseAssignMatchability(ReleaseModel release)
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "UPDATE Release SET matchStatus = @matchStatus WHERE id = @id"
      );
      conn.CommandSetValue("@matchStatus", release.matchStatus);
      conn.CommandSetValue("@id", release.id);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static List<ReleaseModel> GetReleaseNeedCutout()
    {
      // get potential releases
      var dataTable = new DataTable();
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      // matchStatus Matchable, hasCutout false
      conn.CommandSetText(
        "SELECT id, cardSetCode, imageFile, cardNumber FROM Release "
        + "WHERE matchStatus = @matchStatus AND hasCutout = @hasCutout"
      );
      conn.CommandSetValue("@matchStatus", ReleaseModel.MatchStatus.Matchable);
      conn.CommandSetValue("@hasCutout", false);
      conn.CommandExecuteDataTable(dataTable);
      conn.CloseCommand();
      conn.CloseConnection();

      List<ReleaseModel> releases = new List<ReleaseModel>();
      foreach (DataRow row in dataTable.Rows)
      {
        ReleaseModel release = new ReleaseModel();
        release.id = (int)(Int64)row["id"];
        release.cardSetCode = (string)row["cardSetCode"];
        release.imageFile = (string)row["imageFile"];
        release.cardNumber = (string)row["cardNumber"];
        release.hasCutout = release.CutoutFileExists();
        if (release.hasCutout)
        {
          // update database
          ReleaseSetCutout(release);
        }
        else
        {
          releases.Add(release);
        }
      }
      return (releases);
    }

    public static void ReleaseSetCutout(ReleaseModel release)
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "UPDATE Release SET hasCutout = @hasCutout WHERE id = @id"
      );
      conn.CommandSetValue("@hasCutout", release.hasCutout);
      conn.CommandSetValue("@id", release.id);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static void GenerateMatches()
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      // create matches of each cutout cover with each canonical base
      conn.CommandSetText(
        "INSERT OR IGNORE INTO Match( judgment, baseImage, coverImage )" +
        "SELECT @judgment, baseImage, coverImage FROM (" +
          "SELECT Base.id AS 'baseImage' FROM Release AS 'Base' " +
          "JOIN Card ON Base.card = Card.id " +
          "WHERE Card.isBase = 1 " +
          "AND Base.canonicalImage = Base.id " +
        ") CROSS JOIN ( " +
          "SELECT Cover.id AS 'coverImage' FROM Release AS 'Cover' " +
          "JOIN Card ON Cover.card = Card.id " +
          "WHERE Cover.hasCutout = 1 " +
          "AND Card.isBase = 0 " +
        ")"
      );
      conn.CommandSetValue("@judgment", MatchModel.Judgment.Unchecked);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
    }

    public static MatchModel GetMatchNeedJudgment()
    {
      var dataTable = new DataTable();
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "SELECT id, baseImage, coverImage FROM Match "
        + "WHERE judgment == @judgment "
        + "LIMIT 1"
      );
      conn.CommandSetValue("@judgment", MatchModel.Judgment.Unchecked);
      conn.CommandExecuteDataTable(dataTable);
      conn.CloseCommand();
      conn.CloseConnection();

      MatchModel match = new MatchModel();
      if (1 == dataTable.Rows.Count)
      {
        match.id = (int)(Int64)dataTable.Rows[0]["id"];
        match.baseImage = GetReleaseModel(Convert.ToInt32(dataTable.Rows[0]["baseImage"]));
        match.coverImage = GetReleaseModel(Convert.ToInt32(dataTable.Rows[0]["coverImage"]));
      }
      return (match);
    }

    public static void MatchAssignJudgment(MatchModel match)
    {
      DBConnection conn = new DBConnection();
      conn.OpenConnection();
      conn.OpenCommand();
      conn.CommandSetText(
        "UPDATE Match SET judgment = @judgment WHERE id = @id"
      );
      conn.CommandSetValue("@judgment", match.judgment);
      conn.CommandSetValue("@id", match.id);
      conn.CommandExecuteNonQuery();
      conn.CloseCommand();
      conn.CloseConnection();
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
        + "canonicalImage" + NN_REF + "Release(id), "
        + "cardSet" + NN_REF + "CardSet(id), "
        + "card" + NN_REF + "Card(id), "
        + "cardNumber TEXT NOT NULL, "
        + "frame TEXT NOT NULL, "
        + "matchStatus TEXT NOT NULL, "
        + "hasCutout INTEGER NOT NULL, "
        + "cardSetCode TEXT NOT NULL, "
        + "imageFile TEXT NOT NULL, "
        + "UNIQUE (cardSet, cardNumber)"
        + ")",
        CREATE_IF + "Card("
        + "id" + PRIMARY_KEY
        + "isBase INTEGER NOT NULL, "
        + "name TEXT NOT NULL UNIQUE"
        + ")",
        CREATE_IF + "Match("
        + "id" + PRIMARY_KEY
        + "judgment TEXT NOT NULL, "
        + "baseImage" + NN_REF + "Release(id), "
        + "coverImage" + NN_REF + "Release(id), "
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
    }
  }
}
