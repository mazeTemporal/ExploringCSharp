// https://www.codewars.com/kata/55d5434f269c0c3f1b000058/train/csharp

/*
Write a function

TripleDouble(long num1, long num2)
which takes in numbers num1 and num2 and returns 1 if there is a straight triple of a number at any place in num1 and also a straight double of the same number in num2.
For example:
TripleDouble(451999277, 41177722899) == 1 // num1 has straight triple 999s and 
                                          // num2 has straight double 99s

TripleDouble(1222345, 12345) == 0 // num1 has straight triple 2s but num2 has only a single 2

TripleDouble(12345, 12345) == 0

TripleDouble(666789, 12345667) == 1
If this isn't the case, return 0
*/

using System;
using System.Collections.Generic;

public class Kata
{
  public static int TripleDouble(long num1, long num2)
  {
    HashSet<int> straights = FindStraights(num1, 3);
    straights.IntersectWith(FindStraights(num2, 2));
    return (straights.Count > 0 ? 1 : 0);
  }

  public static HashSet<int> FindStraights(long num, int length)
  {
    HashSet<int> straights = new HashSet<int>();
    int prevNum = -1; // intentionally unmatchable value
    int straightLength = 0;
    while (num > 0)
    {
      int thisNum = Convert.ToInt32(num % 10);
      num /= 10;
      if (prevNum == thisNum){
        ++straightLength;
        if (straightLength >= length){
          straights.Add(thisNum);
        }
      } else {
        straightLength = 1;
        prevNum = thisNum;
      }
    }
    return(straights);
  }
}

