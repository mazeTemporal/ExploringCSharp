/*
Definition for simple Binary Tree
*/

public class BinaryTree<T>
{
  public T Value { get; protected set; }

  public BinaryTree<T> LeftChild { get; set; } = null;
  public BinaryTree<T> RightChild { get; set; } = null;

  public BinaryTree(T value)
  {
    Value = value;
  }

  public static void InOrderTraversal(BinaryTree<T> t, Action<T> callback)
  {
    if (t != null)
    {
      InOrderTraversal(t.LeftChild, callback);
      callback(t.Value);
      InOrderTraversal(t.RightChild, callback);
    }
  }

  public static void PreOrderTraversal(BinaryTree<T> t, Action<T> callback)
  {
    if (t != null)
    {
      callback(t.Value);
      PreOrderTraversal(t.LeftChild, callback);
      PreOrderTraversal(t.RightChild, callback);
    }
  }

  public static void PostOrderTraversal(BinaryTree<T> t, Action<T> callback)
  {
    if (t != null)
    {
      PostOrderTraversal(t.LeftChild, callback);
      PostOrderTraversal(t.RightChild, callback);
      callback(t.Value);
    }
  }
}

