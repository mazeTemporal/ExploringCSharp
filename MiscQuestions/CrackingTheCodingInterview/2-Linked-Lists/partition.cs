/*
Partition: Write code to partition a linked list around a value x, such that all nodes less than x come before all nodes greater than or equal to x. If x is contained within the list, the values of x only need to be after the elements less than x. The partition element x can appear anywhere in the "right partition"; it does not need to appear between the left and right partitions.
*/

// O(n) runtime
// O(1) space
public static Node<T> Partitiion<T>(Node<T> n, T partition)
{
  Node<T> left = new Node<T>();
  Node<T> leftTail = left;
  Node<T> right =  new Node<T>();
  Node<T> rightTail = right;

  while (n != null)
  {
    if (Comparer<T>.Default.Compare(n.Data, partition) < 0)
    {
      leftTail.Next = n;
      leftTail = leftTail.Next;
    }
    else
    {
      rightTail.Next = n;
      rightTail = rightTail.Next;
    }
    n = n.Next;
  }

  rightTail.Next = null;
  leftTail.Next = right.Next;
  return left.Next;
}

