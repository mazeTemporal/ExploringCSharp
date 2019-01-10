using System;
using System.Collections.Generic;
namespace CardMatchLibrary.Models
{
  public class CardSetModel
  {
    /// <summary>
    /// Represents the database id.
    /// </summary>
    /// <value>The identifier.</value>
    public int id { get; set; } = -1;

    /// <summary>
    /// Represents the name of a set.
    /// </summary>
    /// <value>The name.</value>
    public string name { get; set; }

    /// <summary>
    /// Represents the abbreviation of the name.
    /// </summary>
    /// <value>The code.</value>
    public string code { get; set; }

    /// <summary>
    /// Represents when the set was released.
    /// </summary>
    /// <value>The release date as YYYY-MM-DD.</value>
    public string releaseDate { get; set; }

    /// <summary>
    /// Represents the cards printed in the set.
    /// </summary>
    /// <value>The releases.</value>
    public List<ReleaseModel> releases { get; set; } = new List<ReleaseModel>();
  }
}
