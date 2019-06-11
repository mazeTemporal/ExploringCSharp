/*
Definition for simple fixed size Stack using Array
*/

public class Stack<T>
{
  private T[] stack;

  private int index = -1;

  public Stack(uint maxSize)
  {
    stack = new T[maxSize];
  }

  public bool IsEmpty() => index < 0;

  public void Push(T val)
  {
    if (index >= stack.Length - 1)
    {
      throw new StackOverflowException();
    }
    index++;
    stack[index] = val;
  }

  public T Pop()
  {
    if (this.IsEmpty())
    {
      throw new StackUnderflowException();
    }
    index--;
    return stack[index + 1];
  }

  public T Peek()
  {
    if (this.IsEmpty())
    {
      throw new StackUnderflowException();
    }
    return stack[index];
  }
}

public class StackUnderflowException : Exception {}

