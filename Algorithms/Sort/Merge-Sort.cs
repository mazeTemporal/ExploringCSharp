/*
Implement Merge Sort.
*/

using System;
using System.Collections.Generic;

namespace AlgorithmSort
{
  public static class Sort
  {
    public static List<T> MergeSort<T>(List<T> t) where T : IComparable<T>
    {
      if (t.Count < 2)
      {
        return (t);
      }
      int halfCount = t.Count / 2;

      return (Merge(
        MergeSort(t.GetRange(0, halfCount)),
        MergeSort(t.GetRange(halfCount, t.Count - halfCount))
      ));
    }

    private static List<T> Merge<T>(List<T> a, List<T> b) where T : IComparable<T>
    {
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

