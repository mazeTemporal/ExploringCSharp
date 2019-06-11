/*
Sum Lists: You have two numbers represented by a linked list, where each node contains a single digit. The digits are stored in reverse order, such that the 1's digit is at the head of the list. Write a function that adds the two numbers and returns the sum as a linked list.
*/

// recursive solution
// O(n) runtime
// O(n) space
public static Node<int> SumList(Node<int> x, Node<int> y, int remainder = 0)
{
  if (x == null && y == null && remainder == 0)
  {
    return null;
  }
  int sum = (x?.Data ?? 0) + (y?.Data ?? 0) + remainder;
  return new Node<int>(sum % 10)
  {
    Next = SumList(x?.Next, y?.Next, sum / 10)
  };
}

