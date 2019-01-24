using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using CardMatchLibrary.Models;

namespace CardMatchLibrary.DataAccess
{
  public static class MTGJson
  {
    public static readonly string PATH = "./Data/MTGJSON/";

    public static string CleanSetCode(string rawCardSetCode)
    {
      return (rawCardSetCode.ToUpper());
    }

    public static string GetMtGSetFilePath(string cardSetCode)
    {
      return (PATH + cardSetCode + ".json");
    }

    public static bool BorderColorIsValid(string borderColor)
    {
      return (
        borderColor == "black" ||
        borderColor == "white"
      );
    }

    public static bool LayoutIsValid(string layout, string side)
    {
      return (
        layout == "normal" ||
        layout == "leveler" ||
        (
          (layout == "transform" && side == "a") ||
          (layout == "meld" && (side == "a" || side == "b"))
        )
      );
    }

    public static bool TypeIsValid(List<string> types)
    {
      return (!types.Contains("Planeswalker"));
    }

    public static bool FrameVersionIsValid(string frameVersion)
    {
      return ("2003" == frameVersion || "2015" == frameVersion);
    }

    public static bool SuperTypeIsBase(List<string> supertypes)
    {
      return (supertypes.Contains("Basic"));
    }

    //!!! needs intregration test
    private static bool JsonCardIsValid(JObject jsonCard)
    {
      return (
        jsonCard.Value<bool>("hasFoil") &&
        BorderColorIsValid(jsonCard.Value<string>("borderColor")) &&
        LayoutIsValid(
          jsonCard.Value<string>("layout"),
          jsonCard.Value<string>("side") ?? ""
        ) &&
        TypeIsValid(jsonCard["types"].ToObject<List<string>>()) &&
        FrameVersionIsValid(jsonCard.Value<string>("frameVersion"))
      );
    }

    //!!! needs integration test
    //!!! refactor to reduce responsibilities
    public static CardSetModel ReadJson(string rawCardSetCode)
    {
      // read json
      string cardSetCode = CleanSetCode(rawCardSetCode);
      string filePath = GetMtGSetFilePath(cardSetCode);
      if (!File.Exists(filePath))
      {
        return (new CardSetModel());
      }
      string json = File.ReadAllText(filePath);
      JObject jsonSet = JObject.Parse(json);
      cardSetCode = CleanSetCode(jsonSet["code"].ToString()); // in case file was misnamed

      // build card set
      CardSetModel cardSet = new CardSetModel();
      cardSet.name = jsonSet["name"].ToString();
      cardSet.code = cardSetCode;
      cardSet.releaseDate = jsonSet["releaseDate"].ToString();

      // build the releases
      foreach (JObject jsonCard in jsonSet["cards"])
      {
        if (JsonCardIsValid(jsonCard))
        {
          CardModel card = new CardModel();
          card.isBase = SuperTypeIsBase(
            jsonCard["supertypes"].ToObject<List<string>>());
          card.name = jsonCard["name"].ToString();

          ReleaseModel release = new ReleaseModel();
          release.card = card;
          release.cardNumber = cardSetCode;
          release.cardNumber = jsonCard["number"].ToString();
          release.imageFile = card.name + ".xlhq.jpg";
          release.frame = jsonCard["frameVersion"].ToString();

          cardSet.releases.Add(release);
        }
      }

      return (cardSet);
    }
  }
}
