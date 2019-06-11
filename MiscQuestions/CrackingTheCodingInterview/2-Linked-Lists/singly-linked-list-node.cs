/*
Definition for simple Singly Linked List
*/

public class Node<T>
{
  public Node<T> Next { get; set; } = null;
  public T Data { get; set; }

  public Node() { }

  public Node(T data)
  {
    Data = data;
  }
}

