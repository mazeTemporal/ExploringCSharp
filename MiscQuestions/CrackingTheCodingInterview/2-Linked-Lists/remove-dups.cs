/*
Remove Dups: Write code to remove duplicates from an unsorted linked list.
*/

// using Singly Linked List


// iterative solution
// O(n) runtime
// O(n) space
public static Node<T> RemoveDups<T>(Node<T> n)
{
  if (n == null)
  {
    return n;
  }
  HashSet<T> unique = new HashSet<T>{ n.Data };
  Node<T> head = n;
  while (n.Next != null)
  {
    if (unique.Contains(n.Next.Data))
    {
      n.Next = n.Next.Next;
    }
    else
    {
      unique.Add(n.Next.Data);
      n = n.Next;
    }
  }
  return head;
}

