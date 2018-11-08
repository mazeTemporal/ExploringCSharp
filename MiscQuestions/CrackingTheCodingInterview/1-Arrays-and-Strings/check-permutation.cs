/*
Check Permutation: Given two strings, write a method to decide if one is a permutation of the other.
*/

static bool IsPermutation(string str1, string str2)
{
  // they are permutations if their sorted characters are the same
  return(SortCharacters(str1) == SortCharacters(str2));
}

static string SortCharacters(string str)
{
  return(str.OrderBy(x => x).Aggregate("", (a, b) => a + b));
}

static bool IsPermutation(string str1, string str2)
{
  // they are permutations if they contain the same counts of each character
  CharCount myCharCount = new CharCount(str1);
  foreach (char c in str2)
  {
    if (!myCharCount.Decrement(c))
    {
      return(false);
    }
  }
  return(0 == myCharCount.Count());
}

class CharCount
{
  private Dictionary<char, int> charDict;
  public CharCount(string str)
  {
    charDict = new Dictionary<char, int>();
    foreach (char c in str)
    {
      Increment(c);
    }
  }

  public int Count()
  {
    return(charDict.Count());
  }

  public void Increment(char c)
  {
    if (charDict.ContainsKey(c))
    {
      ++charDict[c];
    }
    else
    {
      charDict.Add(c, 1);
    }
  }

  // returns if decrement was successful
  public bool Decrement(char c)
  {
    if (charDict.ContainsKey(c))
    {
      --charDict[c];
      if (0 == charDict[c])
      {
        charDict.Remove(c);
      }
      return(true);
    }
    else
    {
      return(false);
    }
  }
}

