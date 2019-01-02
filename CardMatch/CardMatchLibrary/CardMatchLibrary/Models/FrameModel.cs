using System;
namespace CardMatchLibrary.Models
{
  public class FrameModel
  {
    public FrameModel(string frame, int yOffset = 0)
    {
      this.frame = frame;
      this.yOffset = yOffset;
    }

    public FrameModel(int id, string frame, int yOffset)
    {
      this.id = id;
      this.frame = frame;
      this.yOffset = yOffset;
    }

    /// <summary>
    /// Represents the database id.
    /// </summary>
    /// <value>The identifier.</value>
    public int id { get; set; }

    /// <summary>
    /// Represents the art frame.
    /// </summary>
    /// <value>The type of frame format for the card's art box.</value>
    public string frame { get; set; }

    /// <summary>
    /// Represents how far down a cover card should be shifted for match overlay.
    /// </summary>
    /// <value>The y offset in pixels.</value>
    public int yOffset { get; set; }
  }
}
