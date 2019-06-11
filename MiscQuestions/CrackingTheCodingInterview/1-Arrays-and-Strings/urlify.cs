/*
URLify: Write a method to replace all spaces in a string with '%20'. Do the same with a character array. You may assume that the character array has sufficient space at the end to hold additional characters, and that you are given the "true" length of the string.
*/

// using native solution for strings
public static string URLify(string s) => s.Replace(" ", "%20");

// in place option for character arrays
// O(n) runtime
// O(1) space
public static void URLify(char[] s, int stringLength)
{
  // count instances of space character
  int spaceCount = CountChars(s, ' ', 0, stringLength);
  if (spaceCount < 1)
  {
    return;
  }
  int insertPos = stringLength - 1 + 2 * spaceCount;

  for (int readPos = stringLength - 1; readPos >= 0; readPos--)
  {
    if (s[readPos] != ' ')
    {
      s[insertPos] = s[readPos];
      insertPos--;
    }
    else
    {
      foreach (char x in "02%")
      {
        s[insertPos] = x;
        insertPos--;
      }
    }
  }
}

// helper to count occurances of char within subsection of array
// O(n) runtime
// O(1) space
public static int CountChars(char[] s, char c, int startIndex, int length)
{
  if (startIndex < 0 || length < 0 || startIndex + length > s.Length)
  {
    throw new ArgumentException("Section not within array bounds");
  }
  int count = 0;
  for (int i = 0; i < length; i++)
  {
    if (c == s[startIndex + i])
    {
      count++;
    }
  }
  return count;
}

