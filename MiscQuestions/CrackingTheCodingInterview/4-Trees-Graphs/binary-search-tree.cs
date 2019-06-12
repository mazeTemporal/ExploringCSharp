/*
Definition for simple Binary Search Tree
*/

public class BinarySearchTree<T>
{
  public T Value { get; private set; }

  public BinarySearchTree<T> LeftChild { get; private set; } = null;
  public BinarySearchTree<T> RightChild { get; private set; } = null;

  public BinarySearchTree(T value)
  {
    Value = value;
  }

  public void InsertValue(T value)
  {
    if (Comparer<T>.Default.Compare(value, Value) > 0)
    {
      if (RightChild == null)
      {
        RightChild = new BinarySearchTree<T>(value);
      }
      else
      {
        RightChild.InsertValue(value);
      }
    }
    else
    {
      if (LeftChild == null)
      {
        LeftChild = new BinarySearchTree<T>(value);
      }
      else
      {
        LeftChild.InsertValue(value);
      }
    }
  }

  public static void InOrderTraversal(BinarySearchTree<T> t, Action<T> callback)
  {
    if (t != null)
    {
      InOrderTraversal(t.LeftChild, callback);
      callback(t.Value);
      InOrderTraversal(t.RightChild, callback);
    }
  }

  public static void PreOrderTraversal(BinarySearchTree<T> t, Action<T> callback)
  {
    if (t != null)
    {
      callback(t.Value);
      PreOrderTraversal(t.LeftChild, callback);
      PreOrderTraversal(t.RightChild, callback);
    }
  }

  public static void PostOrderTraversal(BinarySearchTree<T> t, Action<T> callback)
  {
    if (t != null)
    {
      PostOrderTraversal(t.LeftChild, callback);
      PostOrderTraversal(t.RightChild, callback);
      callback(t.Value);
    }
  }
}

