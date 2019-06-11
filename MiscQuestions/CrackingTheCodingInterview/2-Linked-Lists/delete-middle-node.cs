/*
Delete Middle Node: Implement an algorithm to delete a node in the middle (any node but the first and last node, not necessarily the exact middle) of a singly linked list, given only access to that node.

Undefined requirements:
  What should it do if the given node is the last node of the list?
  - I will just not delete it
  - Another alternative is to throw an exception
  How will you know if the node is the first in the list if you are not given any other nodes?
  - There is no way to know this, can only assume it is not
*/

// O(1) runtime
// O(1) space
public static Node<T> DeleteNode<T>(Node<T> n) =>
  n == null ? n : n.Next;

