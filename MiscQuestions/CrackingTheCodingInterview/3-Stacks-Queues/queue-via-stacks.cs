/*
Queue via Stacks: Implement a MyQueue class which implements a queue using two stacks.
*/

public class MyQueue<T>
{
  private readonly Stack<T> stack = new Stack<T>();
  private readonly Stack<T> flip = new Stack<T>();

  public bool IsEmpty() => stack.IsEmpty();

  public void Enqueue(T val) => stack.Push(val);

  public T Dequeue()
  {
    Flip(stack, flip);
    T output = flip.Pop(); // could underflow
    Flip(flip, stack);
    return output;
  }

  public T Peek()
  {
    Flip(stack, flip);
    T output = flip.Peek(); // could underflow
    Flip(flip, stack);
    return output;
  }

  private void Flip(Stack<T> from, Stack<T> to)
  {
    while (!from.IsEmpty())
    {
      to.Push(from.Pop());
    }
  }
}

