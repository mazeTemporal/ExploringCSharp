/*
Kth to Last: Implement an algorithm to find the kth to last element of a singly linked list.

Undefined requirements:
  How should it handle lists shorter than k?
    - I will return null
    - alternative is throw an exception
  Is null the last node or are we looking for the last non-null node?
    - I will consider the final null as the last
    - alternative is look ahead one more place
*/

// O(n) runtime
// O(1) space
public static Node<T> KthLast<T>(Node<T> n, int k)
{
  Node<T> ahead = n;
  for (; k > 0; k--)
  {
    if (ahead == null)
    {
      return null;
    }
    ahead = ahead.Next;
  }
  while (ahead != null)
  {
    ahead = ahead.Next;
    n = n.Next;
  }
  return n;  
}
