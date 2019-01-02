using System;
namespace CardMatchLibrary.Models
{
  public class MatchModel
  {
    /// <summary>
    /// Represents the database id.
    /// </summary>
    /// <value>The identifier.</value>
    public int id { get; set; }

    /// <summary>
    /// Represents the base card image.
    /// </summary>
    /// <value>The base image.</value>
    public ReleaseModel baseImage { get; set; }

    /// <summary>
    /// Represents the cover card image.
    /// </summary>
    /// <value>The cover image.</value>
    public ReleaseModel coverImage { get; set; }


    /// <summary>
    /// Possible values for judgement.
    /// </summary>
    public enum Judgment
    {
      Unchecked,
      Match,
      Nonmatch
    };

    /// <summary>
    /// Whether or not this pair is a good match.
    /// </summary>
    /// <value>One of Unchecked, Match, Nonmatch</value>
    public Judgment judgment { get; set; } = Judgment.Unchecked;
  }
}
