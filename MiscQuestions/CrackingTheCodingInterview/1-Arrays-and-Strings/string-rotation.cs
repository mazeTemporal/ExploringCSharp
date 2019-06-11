/*
String Rotation: Assume you have a method isSubstring which checks if one word is a substring of another. Given two strings, s1 and s2, write code to check if s2 is a rotation of s1 using only one call to isSubstring.
Example: "waterbottle" is a rotation of "erbottlewat"
*/

// O(n) runtime
// O(n) space
public static bool IsStringRotation(string s1, string s2)
{
  if (s1.Length != s2.Length)
  {
    return false;
  }
  string combo = s2 + s2;
  // or use "isSubstring" that the instructions provide
  return combo.IndexOf(s1, StringComparison.InvariantCulture) > -1;
}

