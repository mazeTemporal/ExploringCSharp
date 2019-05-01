/*
Count the number of inversions in a list. An inversion is a unique pair of indexes, i and j were i < j and list[i] > list[j].
Example:
  { 1, 3, 2, 1, 2, 4 } has 4 inversions: [3, 2], [3, 1], [3, 2], [2, 1]

Algorithmic Analysis:
  O(n * Log(n)) where n is the length of the list
*/

using System;
using System.Collections.Generic;

namespace Algorithm
{
  public static class Inversion
  {
    public static int InversionCount<T>(List<T> t) where T : IComparable<T>
    {
      InversionCountRecursive(t, out int inversion);
      return (inversion);
    }

    private static List<T> InversionCountRecursive<T>(List<T> t, out int inversion) where T : IComparable<T>
    {
      if (t.Count < 2)
      {
        inversion = 0;
        return (t);
      }
      int halfCount = t.Count / 2;

      List<T> result = MergeCount(
        InversionCountRecursive(t.GetRange(0, halfCount), out int leftInversion),
        InversionCountRecursive(t.GetRange(halfCount, t.Count - halfCount), out int rightInversion),
        out int crossInversion
      );

      inversion = leftInversion + rightInversion + crossInversion;

      return (result);
    }

    private static List<T> MergeCount<T>(List<T> a, List<T> b, out int inversion) where T : IComparable<T>
    {
      inversion = 0;
      List<T> result = new List<T>();
      int i = 0;
      int j = 0;

      // merge
      while (i < a.Count && j < b.Count)
      {
        if (a[i].CompareTo(b[j]) < 1)
        {
          result.Add(a[i]);
          i++;
        }
        else
        {
          result.Add(b[j]);
          inversion += a.Count - i;
          j++;
        }
      }

      // concatenate remaining values
      if (i < a.Count)
      {
        result.AddRange(a.GetRange(i, a.Count - i));
      }
      else
      {
        result.AddRange(b.GetRange(j, b.Count - j));
      }

      return (result);
    }
  }
}

