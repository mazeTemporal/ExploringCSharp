using System;
using System.Collections.Generic;

namespace Offline
{
  public class Card
  {
    static HashSet<string> validFrames = new HashSet<string>
    {
      "2003",
      "2015"
    };
    static HashSet<string> validLayouts = new HashSet<string>
    {
      "normal",
      "transform",
      "leveler"
    };
    static HashSet<string> validBorders = new HashSet<string>
    {
      "black",
      "white"
    };
    public bool hasFoil;
    public bool isOnlineOnly;
    public bool isOversized;
    public string frameVersion;
    public string borderColor;
    public string layout;
    public string name;
    public string number;
    public List<string> supertypes;
    public List<string> types;

    public bool IsBase()
    {
      return (supertypes.Contains("Basic"));
    }

    public bool IsValid()
    {
      return (
        hasFoil &&
        !isOnlineOnly &&
        !isOversized &&
        FrameIsValid()
      );
    }

    public string GetName()
    {
      return (name);
    }

    public string GetFrame()
    {
      return (frameVersion);
    }

    public string GetNumber()
    {
      return (number);
    }

    bool FrameIsValid()
    {
      return (
        validFrames.Contains(frameVersion) &&
        validBorders.Contains(borderColor) &&
        !types.Contains("Planeswalker") &&
        LayoutIsValid()
      );
    }

    bool LayoutIsValid()
    {
      return (
        validLayouts.Contains(layout) ||
        ("meld" == layout && 'a' == number[number.Length - 1])
      );
         }
  }
}
