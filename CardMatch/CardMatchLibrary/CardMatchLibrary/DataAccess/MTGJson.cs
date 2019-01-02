using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using CardMatchLibrary.Models;

namespace CardMatchLibrary.DataAccess
{
  public static class MTGJson
  {
    private static string CleanSetCode(string rawCardSetCode)
    {
      return (rawCardSetCode.ToUpper());
    }

    private static string GetMtGSetFilePath(string cardSetCode)
    {
      const string PATH = "./Data/MTGJSON/";
      return (PATH + cardSetCode + ".json");
    }

    private static bool BorderColorIsValid(JObject jsonCard)
    {
      string borderColor = jsonCard.Value<string>("borderColor");
      return (
        borderColor == "black" ||
        borderColor == "white"
      );
    }

    private static bool LayoutIsValid(JObject jsonCard)
    {
      string layout = jsonCard.Value<string>("layout");
      string side = jsonCard.Value<string>("side") ?? "";
      return (
        layout == "normal" ||
        layout == "leveler" ||
        (
          (layout == "transform" && side == "a") ||
          (layout == "meld" && (side == "a" || side == "b"))
        )
      );
    }

    private static bool TypeIsValid(JObject jsonCard)
    {
      return (
        !jsonCard["types"].ToObject<List<string>>().Contains("Planeswalker")
      );
    }

    private static bool FrameVersionIsValid(JObject jsonCard)
    {
      string frameVersion = jsonCard.Value<string>("frameVersion");
      return (-1 < DataConnector.GetFrameModel(frameVersion).id);
    }

    private static bool JsonCardIsValid(JObject jsonCard)
    {
      return (
        jsonCard.Value<bool>("hasFoil") &&
        BorderColorIsValid(jsonCard) &&
        LayoutIsValid(jsonCard) &&
        TypeIsValid(jsonCard) &&
        FrameVersionIsValid(jsonCard)
      );
    }

    private static bool JsonCardIsBase(JObject jsonCard)
    {
      return(jsonCard["supertypes"].ToObject<List<string>>().Contains("Basic"));
    }

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
          card.isBase = JsonCardIsBase(jsonCard);
          card.name = jsonCard["name"].ToString();

          ReleaseModel release = new ReleaseModel(cardSetCode);
          release.card = card;
          release.cardNumber = jsonCard["number"].ToString();
          release.imageFile = card.name + ".xlhq.jpg";
          release.frame = DataConnector.GetFrameModel(
            jsonCard["frameVersion"].ToString());

          cardSet.releases.Add(release);
        }
      }

      return (cardSet);
    }
  }
}
