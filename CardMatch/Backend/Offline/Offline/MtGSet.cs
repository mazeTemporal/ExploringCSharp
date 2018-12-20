using System;
using System.Collections.Generic;
using System.Linq;

namespace Offline
{
  public class MtGSet
  {
    public List<Card> cards;
    public string code;
    public string name;
    public string releaseDate;

    public void FilterValid()
    {
      cards = cards.Where(x => x.IsValid()).ToList();
    }

    public List<Card> GetCards()
    {
      return (cards);
    }

    public string GetCode()
    {
      return (code.ToUpper());
    }

    public string GetName()
    {
      return (name);
    }

    public string GetDate()
    {
      return (releaseDate);
    }
  }
}
