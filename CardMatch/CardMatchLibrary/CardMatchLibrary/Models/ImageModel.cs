﻿using System;
using System.IO;

namespace CardMatchLibrary.Models
{
  public class ImageModel
  {
    public ImageModel(string cardSetCode)
    {
      this.cardSetCode = cardSetCode;
    }

    /// <summary>
    /// Path to image folder.
    /// </summary>
    private const string IMAGE_PATH = "./Data/Image/";

    /// <summary>
    /// Represents the database id.
    /// </summary>
    /// <value>The identifier.</value>
    public int id { get; set; }

    /// <summary>
    /// Represents the name of the image file.
    /// </summary>
    /// <value>The name of the file.</value>
    private string _fileName;
    public string fileName
    {
      get { return (_fileName); }
      set
      {
        _fileName = File.Exists(GetPath(value)) ? value : "";
      }
    }

    /// <summary>
    /// Gets or sets the card set code.
    /// </summary>
    /// <value>The card set code.</value>
    public string cardSetCode { get; set; }

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

    /// <summary>
    /// Gets path to image file.
    /// </summary>
    /// <returns>The file path.</returns>
    public string GetPath(string name = null)
    {
      return (IMAGE_PATH + cardSetCode + "/" + (null == name ? fileName : name));
    }
  }
}
