/*
Intersection: Given two singly linked lists, determine if the two lists intersect. Return the intersecting node. Note that the intersection is defined based on reference, not value. That is, if the kth node of the first linked list is the exact same node by reference as the jth node of the second linked list, then they are intersecting.

Undefined requirements:
  What should be returned if they are not intersecting?
  - I will return null.
*/

// O(n) runtime
// O(n) space
public static Node<T> GetIntersection<T>(Node<T> x, Node<T> y)
{
  List<Node<T>> xList = ListifyNodes(x);
  List<Node<T>> yList = ListifyNodes(y);
  xList.Reverse();
  yList.Reverse();

  Node<T> output = null;
  for (int i = 0; i < xList.Count && i < yList.Count; i++)
  {
    if (!Object.ReferenceEquals(xList[i], yList[i]))
    {
      break;
    }
    output = xList[i];
  }

  return output;
}

// O(n) runtime
// O(n) space
public static List<Node<T>> ListifyNodes<T>(Node<T> x)
{
  List<Node<T>> output = new List<Node<T>>();
  while (x != null)
  {
    output.Add(x);
    x = x.Next;
  }
  output.Add(null);
  return output;
}

