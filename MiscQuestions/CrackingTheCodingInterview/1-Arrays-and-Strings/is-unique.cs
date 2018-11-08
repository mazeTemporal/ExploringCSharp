/*
Is Unique: Implement an algorithm to determine if a string has all unique characters. What if you cannot use additional data structures?
*/

static bool IsUnique(string str)
{
  // push characters into a set, break if present
  HashSet<char> charSet = new HashSet<char>();
  foreach (char c in str)
  {
    if (charSet.Contains(c)){
      return(false);
    }
    charSet.Add(c);
  }
  return(true);
}

static bool IsUnique(string str)
{
  // alternative using Linq
  return(str.Length == str.Distinct().Count());
}

// strings are immutable in c# so characters cannot be sorted
// without using additional data structures
static bool IsUnique(string str)
{
  // check if any character matches a later character
  for (int i = 0; i < str.Length; ++i)
  {
    for (int j = i + 1; j < str.Length; ++j)
    {
      if (str[i] == str[j])
      {
        return(false);
      }
    }
  }
  return(true);
}

