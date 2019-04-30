/*
Perform addition and subtraction on IntegerStrings.

Algorithmic Analysis:
  Add and Subtract
    O(max(n, m)) where n and m are the number of digits in the operands
*/

using System.Collections.Generic;

namespace AlgorithmMath
{
  public partial class IntegerString
  {
    public void Add(string s)
    {
      Add(new IntegerString(s));
    }

    public void Subtract(string s)
    {
      Subtract(new IntegerString(s));
    }

    public void Add(IntegerString s)
    {
      SetSelfEqual(Add(this, s));
    }

    public void Subtract(IntegerString s)
    {
      SetSelfEqual(Subtract(this, s));
    }

    public static string Add(string x, string y)
    {
      return (Add(new IntegerString(x), new IntegerString(y)).ToString());
    }

    public static string Subtract(string x, string y)
    {
      return (Subtract(new IntegerString(x), new IntegerString(y)).ToString());
    }

    public static IntegerString Add(IntegerString x, IntegerString y)
    {
      // adding a negative number is subtracting
      if (x.isNegative)
      {
        return (Subtract(y, GetInverse(x)));
      }
      if (y.isNegative)
      {
        return (Subtract(x, GetInverse(y)));
      }

      List<int> result = new List<int>();
      int carryOver = 0;

      // add each digit
      for (int i = 0; i < x.Count || i < y.Count; i++)
      {
        int digitSum = carryOver;
        if (i < x.Count)
        {
          digitSum += x[i];
        }
        if (i < y.Count)
        {
          digitSum += y[i];
        }
        result.Add(digitSum % 10);
        carryOver = digitSum / 10;
      }

      // could have carry over from final addition
      if (carryOver > 0)
      {
        result.Add(carryOver);
      }

      return (new IntegerString(result, false));
    }

    public static IntegerString Subtract(IntegerString x, IntegerString y)
    {
      // subtracting a negative number is adding
      if (y.isNegative)
      {
        return (Add(x, GetInverse(y)));
      }
      // subtracting from a negative number is like adding
      if (x.isNegative)
      {
        return (GetInverse(Add(GetInverse(x), y)));
      }
      // simpler if big number on top
      if (x < y)
      {
        return (GetInverse(Subtract(y, x)));
      }

      List<int> result = new List<int>();
      bool overflow = false;

      // add each digit
      for (int i = 0; i < x.Count || i < y.Count; i++)
      {
        int digitSum = overflow ? -1 : 0;
        if (i < x.Count)
        {
          digitSum += x[i];
        }
        if (i < y.Count)
        {
          digitSum -= y[i];
        }
        result.Add((10 + digitSum) % 10);
        overflow = digitSum < 0;
      }

      return (new IntegerString(result, false));
    }

    public static IntegerString operator +(IntegerString x, IntegerString y)
    {
      return (Add(x, y));
    }

    public static IntegerString operator -(IntegerString x, IntegerString y)
    {
      return (Subtract(x, y));
    }
  }
}

