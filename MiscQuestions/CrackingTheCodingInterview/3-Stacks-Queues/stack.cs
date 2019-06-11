/*
Definition for simple Stack using List
*/

public class Stack<T>
{
  private readonly List<T> stack = new List<T>();

  public void Push(T val) => stack.Add(val);

  public bool IsEmpty() => stack.Count == 0;

  public T Pop()
  {
    if (this.IsEmpty())
    {
      throw new StackUnderflowException();
    }
    T output = stack[stack.Count - 1];
    stack.RemoveAt(stack.Count - 1);
    return output;
  }

  public T Peek()
  {
    if (this.IsEmpty())
    {
      throw new StackUnderflowException();
    }
    return stack[stack.Count - 1];
  }
}

public class StackUnderflowException : Exception {}

