/*
Stack Min: How would you design a stack which, in addition to push and pop, has a function min which returns the minimum element? Push, pop, and min shoul all operate in O(1) time.
*/

public class StackMin<T>
{
  private readonly List<T> stack = new List<T>();
  private readonly List<T> min = new List<T>();

  public void Push(T val)
  {
    stack.Add(val);
    bool isNewMin = IsEmpty() ||
      Comparer<T>.Default.Compare(val, min[min.Count - 1]) < 0;
    min.Add(isNewMin ? val : min[min.Count - 1]);
  }

  public bool IsEmpty() => IsEmpty(stack);

  private static bool IsEmpty(List<T> x) => x.Count == 0;

  private static void RemoveLast(List<T> x) => x.RemoveAt(x.Count - 1);

  public T Pop()
  {
    T output = Peek(stack);
    RemoveLast(stack);
    RemoveLast(min);
    return output;
  }

  private static T Peek(List<T> x)
  {
    if (IsEmpty(x))
    {
      throw new StackUnderflowException();
    }
    return x[x.Count - 1];
  }

  public T Peek() => Peek(stack);

  public T Min() => Peek(min);
}

public class StackUnderflowException : Exception {}

