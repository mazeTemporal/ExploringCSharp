/*
Palindrome Permutation: Given a string, write a function to check if it is a permutation of a palindrome.
*/

// O(n) runtime
// O(1) space
public static bool IsPalindromePermutation(string s)
{
  HashSet<char> toggle = new HashSet<char>();
  foreach (char c in s)
  {
    if (' ' != c) // ignore spaces
    {
      ToggleValue(toggle, char.ToLower(c));
    }
  }
  return toggle.Count < 2;
}

// could be refactored to a method of a Toggle<T> container class
public static void ToggleValue<T>(HashSet<T> toggle, T val)
{
  if (toggle.Contains(val))
  {
    toggle.Remove(val);
  }
  else
  {
    toggle.Add(val);
  }
}

