/*
String Compression: Implement a method to perform basic string compression using the counts of repeated characters. For example, the string aabcccccaaa would become a2b1c5a3. If the "compressed" string would not become smaller than the original string, instead return the original string. You can assume the string has only uppercase and lowercase letters(a-z).
*/

// O(n) runtime
// O(n) space
public static string CompressString(string s)
{
  if (s.Length < 2)
  {
    return s;
  }

  StringBuilder output = new StringBuilder(s.Length);
  int count = 1;

  for (int i = 1; i < s.Length; i++)
  {
    if (s[i] == s[i - 1])
    {
      count++;
    }
    else
    {
      output.Append(s[i - 1]);
      output.Append(count);
      count = 1;
    }
  }

  output.Append(s[s.Length - 1]);
  output.Append(count);

  return output.Length < s.Length ? output.ToString() : s;
}

