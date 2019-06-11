/*
Sort Stack: Write a program to sort a stack such that the smallest items are on the top. You can use an additional temporary stack, but may not copy the elements into any other data structure (such as an array). The stack supports the following operations: push, pop, peek, and isEmpty.
*/

// O(n^2) runtime
// O(n) space
public static void SortStack<T>(Stack<T> stack)
{
  Stack<T> max = new Stack<T>();
  Stack<T> low = new Stack<T>();
  Stack<T> temp = new Stack<T>();

  // set up
  DumpStack(stack, temp);

  while (!max.IsEmpty() || !temp.IsEmpty())
  {
    while (!temp.IsEmpty())
    {
      T val = temp.Pop();
      if (max.IsEmpty() || Comparer<T>.Default.Compare(val, max.Peek()) > 0)
      {
        max.Push(val);
      }
      else
      {
        low.Push(val);
      }
    }
    stack.Push(max.Pop());
    DumpStack(low, temp);
  }
}

// O(n) runtime
// O(1) space
public static void DumpStack<T>(Stack<T> from, Stack<T> to)
{
  while (!from.IsEmpty())
  {
    to.Push(from.Pop());
  }
}
