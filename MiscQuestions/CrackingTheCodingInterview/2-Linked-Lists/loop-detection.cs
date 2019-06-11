/*
Loop Detection: Given a circular linked list, implement an algorithm that returns the node at the beginning of the loop.

Undefined requirements:
  What should be returned if there is no loop?
  - I will return null
*/

public static Node<T> FindLoop<T>(Node<T> n)
{
  HashSet<Node<T>> visited = new HashSet<Node<T>>();
  while (n != null)
  {
    if (visited.Contains(n.Next))
    {
      return n.Next;
    }
    visited.Add(n);
    n = n.Next;
  }
  return null;
}

