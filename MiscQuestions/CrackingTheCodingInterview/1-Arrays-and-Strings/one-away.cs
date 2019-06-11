/*
One Away: There are three types of edits that can be performed on strings: insert a character, remove a character, or replace a character. Given two strings, write a function to check if they are one edit (or zero edits) away.
Examples:
pale, pale -> true // zero edits
pale, pales -> true // insertion
pale, ple -> true // deletion
pale, bale -> true // replacement
pale, bake -> false // two replacement
pale, ble -> false // replacement and deletion	
*/

// O(n) runtime
// O(1) space
public static bool OneAway(string x, string y)
{
  if (x.Length == y.Length)
  {
    return OneReplacement(x, y);
  }
  return OneInsertion(
    x.Length > y.Length ? x : y, // longer string
    x.Length > y.Length ? y : x  // shorter string
  );
}

// O(n) runtime
// O(1) space
public static bool OneReplacement(string x, string y)
{
  if (x.Length != y.Length)
  {
    return false;
  }
  bool replaced = false;
  for (int i = 0; i < x.Length; i++)
  {
    if (x[i] != y[i])
    {
      if (replaced)
      {
        return false;
      }
      replaced = true;
    }
  }
  return true;
}

// O(n) runtime
// O(1) space
public static bool OneInsertion(string big, string small)
{
  if (big.Length - small.Length != 1)
  {
    return false;
  }
  int offset = 0;
  for (int i = 0; i < small.Length; i++)
  {
    if (big[i + offset] != small[i])
    {	
      if (offset > 0)
      {
        return false;
      }
      offset++;
      i--;
    }
  }
  return true;
}

