/*
List of Depths: Given a binary tree, design an algorithm which creates a linked list of all the nodes at each depth (e.g., if you have a tree with depth D, you'll have D linked lists).
*/

// O(n) runtime
// O(n) space
public static List<LinkedList<BinaryTree<T>>> ListOfDepths<T>(BinaryTree<T> t)
{
  List<LinkedList<BinaryTree<T>>> output = new List<LinkedList<BinaryTree<T>>>();
  ListOfDepths(output, t, 1);
  return output;
}

// O(n) runtime
// O(n) space
private static void ListOfDepths<T>(List<LinkedList<BinaryTree<T>>> output, BinaryTree<T> t, int depth)
{
  if (t != null)
  {
    if (output.Count < depth)
    {
      output.Add(new LinkedList<BinaryTree<T>>());
    }
    output[depth - 1].AddLast(t);
    ListOfDepths(output, t.LeftChild, depth + 1);
    ListOfDepths(output, t.RightChild, depth + 1);
  }
}

