// https://www.codewars.com/kata/moving-zeros-to-the-end/train/csharp

/*
Write an algorithm that takes an array and moves all of the zeros to the end, preserving the order of the other elements.

Kata.MoveZeroes(new int[] {1, 2, 0, 1, 0, 1, 0, 3, 0, 1}) => new int[] {1, 2, 1, 1, 3, 1, 0, 0, 0, 0}
*/

using System;

public class Kata
{
  public static int[] MoveZeroes(int[] arr)
  {
    int[] sorted = new int[arr.Length];
    int j = 0;
    // fill in nonzeros
    for (int i = 0; i < arr.Length; ++i)
    {
      if (0 != arr[i])
      {
        sorted[j] = arr[i];
        ++j;
      }
    }
    // remainder must be zeros
    for (; j < sorted.Length; ++j)
    {
      sorted[j] = 0;
    }
    return (sorted);
  }
}

