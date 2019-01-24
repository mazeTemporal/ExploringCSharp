using System;
using Xunit;
using CardMatchLibrary.DataAccess;
using System.Collections.Generic;
namespace CardMatchLibrary.Test.DataAccess
{
  public class MTGJsonTest
  {
    [Fact]
    public void CleanSetCode_Capitalizes()
    {
      string expected = "AK3";
      string actual = MTGJson.CleanSetCode("ak3");
      Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetMtGSetFilePath_Works()
    {
      string cardSetCode = "UML";
      string expected = MTGJson.PATH + cardSetCode + ".json";
      string actual = MTGJson.GetMtGSetFilePath("UML");
      Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("black")]
    [InlineData("white")]
    public void BorderColorIsValid_TrueForGoodValues(string borderColor)
    {
      bool actual = MTGJson.BorderColorIsValid(borderColor);
      Assert.True(actual);
    }

    [Theory]
    [InlineData("silver")]
    [InlineData("gold")]
    [InlineData("borderless")]
    public void BorderColorIsValid_FalseForBadValues(string borderColor)
    {
      bool actual = MTGJson.BorderColorIsValid(borderColor);
      Assert.False(actual);
    }

    [Theory]
    [InlineData("normal")]
    [InlineData("leveler")]
    public void LayoutIsValid_TrueForGoodLayoutAnySide(string layout)
    {
      string[] sides = { "", "a", "b", "c" };
      foreach (string side in sides)
      {
        bool actual = MTGJson.LayoutIsValid(layout, side);
        Assert.True(actual);
      }
    }

    [Theory]
    [InlineData("transform", "a")]
    [InlineData("meld", "a")]
    [InlineData("meld", "b")]
    public void LayoutIsValid_TrueForPartialLayoutGoodSide(string layout, string side)
    {
      bool actual = MTGJson.LayoutIsValid(layout, side);
      Assert.True(actual);
    }

    [Theory]
    [InlineData("split")]
    [InlineData("flip")]
    [InlineData("saga")]
    [InlineData("planar")]
    [InlineData("scheme")]
    [InlineData("vanguard")]
    [InlineData("token")]
    [InlineData("double_faced_token")]
    [InlineData("emblem")]
    [InlineData("augment")]
    [InlineData("host")]
    public void LayoutIsValid_FalseForBadLayoutAnySide(string layout)
    {
      string[] sides = { "", "a", "b", "c" };
      foreach (string side in sides)
      {
        bool actual = MTGJson.LayoutIsValid(layout, side);
        Assert.False(actual);
      }
    }

    [Theory]
    [InlineData("transform", "")]
    [InlineData("transform", "b")]
    [InlineData("transform", "c")]
    [InlineData("meld", "")]
    [InlineData("meld", "c")]
    public void LayoutIsValid_FalseForPartialLayoutBadSide(string layout, string side)
    {
      bool actual = MTGJson.LayoutIsValid(layout, side);
      Assert.False(actual);
    }

    [Theory]
    [InlineData()]
    [InlineData("Instant")]
    [InlineData("Artifact", "Creature")]
    public void TypeIsValid_TrueIfNotContainsPlaneswalker(params string[] typeArray)
    {
      List<string> types = new List<string>(typeArray);
      bool actual = MTGJson.TypeIsValid(types);
      Assert.True(actual);
    }

    [Theory]
    [InlineData("Planeswalker")]
    [InlineData("Instant", "Planeswalker", "Artifact")]
    public void TypeIsValid_FalseIfContainsPlaneswalker(params string[] typeArray)
    {
      List<string> types = new List<string>(typeArray);
      bool actual = MTGJson.TypeIsValid(types);
      Assert.False(actual);
    }

    [Theory]
    [InlineData("2003")]
    [InlineData("2015")]
    public void FrameVersionIsValid_TrueForGoodValues(string frameVersion)
    {
      bool actual = MTGJson.FrameVersionIsValid(frameVersion);
      Assert.True(actual);
    }

    [Theory]
    [InlineData("1993")]
    [InlineData("1997")]
    [InlineData("future")]
    public void FrameVersionIsValid_FalseForBadValues(string frameVersion)
    {
      bool actual = MTGJson.FrameVersionIsValid(frameVersion);
      Assert.False(actual);
    }

    [Theory]
    [InlineData("Basic")]
    [InlineData("Snow", "Basic", "Global")]
    public void SuperTypeIsBase_TrueIfContainsBasic(params string[] supertypeArray)
    {
      List<string> supertypes = new List<string>(supertypeArray);
      bool actual = MTGJson.SuperTypeIsBase(supertypes);
      Assert.True(actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Global", "Legendary")]
    public void SuperTypeIsBase_FalseIfNotContainsBasic(params string[] supertypeArray)
    {
      List<string> supertypes = new List<string>(supertypeArray);
      bool actual = MTGJson.SuperTypeIsBase(supertypes);
      Assert.False(actual);
    }
  }
}
