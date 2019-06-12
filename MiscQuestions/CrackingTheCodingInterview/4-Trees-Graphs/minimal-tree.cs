/*
Minimal Tree: Given a sorted (increasing order) array with unique integer elements, write an algorithm to create a binary search tree with minimal height.
*/

// O(n) runtime
// O(n) space
public static BinarySearchTree<T> MinimalTree<T>(T[] elements) =>
  MinimalTree(elements, 0, elements.Length - 1);

// O(n) runtime
// O(n) space
private static BinarySearchTree<T> MinimalTree<T>(T[] elements, int startIndex, int endIndex)
{
  if (startIndex > endIndex)
  {
    return null;
  }
  int center = (startIndex + endIndex) / 2;
  return new BinarySearchTree<T>(elements[center])
  {
    LeftChild = MinimalTree(elements, startIndex, center - 1),
    RightChild = MinimalTree(elements, center + 1, endIndex)
  };
}

