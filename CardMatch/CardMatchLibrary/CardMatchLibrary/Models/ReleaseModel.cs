using System;
using System.IO;
namespace CardMatchLibrary.Models
{
  public class ReleaseModel
  {
    /// <summary>
    /// Path to image folder.
    /// </summary>
    private const string IMAGE_PATH = "./Data/Image/";

    /// <summary>
    /// Represents the database id.
    /// </summary>
    /// <value>The identifier.</value>
    public int id { get; set; } = -1;

    /// <summary>
    /// Represents the card's collector number.
    /// </summary>
    /// <value>The card number.</value>
    public string cardNumber { get; set; }

    /// <summary>
    /// Represents the nonphysical details of the card.
    /// </summary>
    /// <value>The card.</value>
    public CardModel card { get; set; }

    /// <summary>
    /// Represents the name of the image file.
    /// </summary>
    /// <value>The name of the file.</value>
    public string imageFile { get; set; }

    /// <summary>
    /// Gets or sets the card set code.
    /// </summary>
    /// <value>The card set code.</value>
    public string cardSetCode { get; set; }

    /// <summary>
    /// Gets or sets the card set code.
    /// </summary>
    /// <value>The card set code.</value>
    public int cardSetId { get; set; } = -1;

    /// <summary>
    /// Represents the canonical version of duplicate card arts.
    /// </summary>
    /// <value>The canonical image id. -1 means not assigned.</value>
    public int canonicalImageId { get; set; } = -1;

    /// <summary>
    /// Represents the format of the art box.
    /// </summary>
    /// <value>The art frame.</value>
    public FrameModel frame { get; set; }

    /// <summary>
    /// Possible values for match status.
    /// </summary>
    public enum MatchStatus
    {
      Unchecked,
      Matchable,
      Unmatchable
    };

    /// <summary>
    /// Whether or not this cover could be a match.
    /// </summary>
    /// <value>One of Unchecked, Matchable, Unmatchable</value>
    public MatchStatus matchStatus { get; set; } = MatchStatus.Unchecked;

    /// <summary>
    /// Has the cover cutout image been created?
    /// </summary>
    /// <value>True if the cutout exists.</value>
    public bool hasCutout { get; set; } = false;

    public void VerifyImageFile()
    {
      if (!File.Exists(GetPath(imageFile)))
      {
        imageFile = "";
      }
    }

    /// <summary>
    /// Gets path to image file.
    /// </summary>
    /// <returns>The file path.</returns>
    public string GetPath(string name = null)
    {
      return (IMAGE_PATH + cardSetCode + "/" + (null == name ? imageFile : name));
    }

  }
}
