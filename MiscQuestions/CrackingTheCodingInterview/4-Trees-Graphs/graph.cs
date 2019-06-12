/*
Definition for simple Graphs using a Set
*/

public abstract class GraphNode<T>
{
  public T Value { get; set; }

  protected HashSet<GraphNode<T>> connections = new HashSet<GraphNode<T>>();

  protected GraphNode(T value)
  {
    Value = value;
  }

  public List<GraphNode<T>> GetConnections() => connections.ToList();
}

public class UndirectedGraph<T> : GraphNode<T>
{
  public UndirectedGraph(T value) : base(value) { }

  public void AddConnection(UndirectedGraph<T> node)
  {
    connections.Add(node);
    node.connections.Add(this);
  }

  public void RemoveConnection(UndirectedGraph<T> node)
  {
    connections.Remove(node);
    node.connections.Remove(this);
  }
}

public class DirectedGraph<T> : GraphNode<T>
{
  public DirectedGraph(T value) : base(value) { }

  public void AddConnection(DirectedGraph<T> node)
  {
    connections.Add(node);
  }

  public void RemoveConnection(DirectedGraph<T> node)
  {
    connections.Remove(node);
  }
}

