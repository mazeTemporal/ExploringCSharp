/*
Definition for simple Stack using Linked List
*/

public class Stack<T>
{
  private Node<T> head = null;

  public bool IsEmpty() => head == null;

  public void Push(T val)
  {
    head = new Node<T>
    {
      Data = val,
      Next = head
    };
  }

  public T Pop()
  {
    if (this.IsEmpty())
    {
      throw new StackUnderflowException();
    }
    T output = head.Data;
    head = head.Next;
    return output;
  }

  public T Peek()
  {
    if (this.IsEmpty())
    {
      throw new StackUnderflowException();
    }
    return head.Data;
  }
}

public class StackUnderflowException : Exception {}

