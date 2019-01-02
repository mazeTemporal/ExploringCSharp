using System;
namespace CardMatchLibrary.Models
{
  public class CardModel
  {
    /// <summary>
    /// Represents the database id.
    /// </summary>
    /// <value>The identifier.</value>
    public int id { get; set; }

    /// <summary>
    /// If the card is treated as the base or cover when matching.
    /// </summary>
    /// <value>True if it is a base, false if it is a cover.</value>
    public bool isBase { get; set; }

    /// <summary>
    /// Name of the card.
    /// </summary>
    /// <value>The name.</value>
    public string name { get; set; }
  }
}
