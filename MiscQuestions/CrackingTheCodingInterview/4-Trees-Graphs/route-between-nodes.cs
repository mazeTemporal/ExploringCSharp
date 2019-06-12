/*
Route Between Nodes: Given a directed graph, design an algorithm to find out whether there is a route between two nodes.
*/

// O(n) runtime
// O(n) space
public static bool RouteBetweenNodes<T>(DirectedGraph<T> x, DirectedGraph<T> y)
{
  return RouteToNode(x, y) || RouteToNode(y, x);
}

// O(n) runtime
// O(n) space
public static bool RouteToNode<T>(DirectedGraph<T> fromNode, DirectedGraph<T> toNode)
{
  // use a breadth first search
  HashSet<DirectedGraph<T>> checkedNodes = new HashSet<DirectedGraph<T>>();
  Queue<DirectedGraph<T>> nodesToCheck = new Queue<DirectedGraph<T>>();
  nodesToCheck.Enqueue(fromNode);

  while (nodesToCheck.Count > 0)
  {
    DirectedGraph<T> currentNode = nodesToCheck.Dequeue();
    if (!checkedNodes.Contains(currentNode))
    {
      if (Object.ReferenceEquals(currentNode, toNode))
      {
        return true;
      }
      checkedNodes.Add(currentNode);
      foreach (DirectedGraph<T> connection in currentNode.GetConnections())
      {
        nodesToCheck.Enqueue(connection);
      }
    }
  }
  return false;
}

