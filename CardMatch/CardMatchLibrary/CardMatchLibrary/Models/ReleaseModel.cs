using System;
namespace CardMatchLibrary.Models
{
  public class ReleaseModel
  {
    /// <summary>
    /// Represents the database id.
    /// </summary>
    /// <value>The identifier.</value>
    public int id { get; set; }

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
    /// Represents the appearance of the card.
    /// </summary>
    /// <value>The card image.</value>
    public ImageModel image { get; set; }
  }
}
