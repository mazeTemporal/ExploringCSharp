/*
Palindrome: Implement a function to check if a linked list is a palindrome.
*/

// O(n) runtime
// O(n) space
public static bool IsPalindrome<T>(Node<T> n) =>
  IsPalindrome(LinkedListToList(n));

// O(n) runtime
// O(n) space
public static List<T> LinkedListToList<T>(Node<T> n)
{
  List<T> output = new List<T>();
  while (n != null)
  {
    output.Add(n.Data);
    n = n.Next;
  }
  return output;
}

// O(n) runtime
// O(1) space
public static bool IsPalindrome<T>(List<T> x)
{
  for (int i = 0; i < x.Count / 2; i++)
  {
    if (Comparer<T>.Default.Compare(x[i], x[x.Count - i - 1]) != 0)
    {
      return false;
    }
  }
  return true;
}

