/*
Perform multiplication on IntegerStrings using Karatsuba algorithm.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmMath
{
  public partial class IntegerString
  {
    private void DigitShift(int shift)
    {
      if (Count > 1 || this[0] != 0)
      {
        _digits.InsertRange(0, Enumerable.Repeat(0, shift));
      }
    }

    private static IntegerString DigitShift(IntegerString x, int shift)
    {
      IntegerString y = new IntegerString(x);
      y.DigitShift(shift);
      return (y);
    }

    public void MultiplyKaratsuba(string s)
    {
      MultiplyKaratsuba(new IntegerString(s));
    }

    public void MultiplyKaratsuba(IntegerString x)
    {
      SetSelfEqual(MultiplyKaratsuba(this, x));
    }

    public static string MultiplyKaratsuba(string x, string y)
    {
      return (MultiplyKaratsuba(new IntegerString(x), new IntegerString(y)).ToString());
    }

    public static IntegerString MultiplyKaratsuba(IntegerString x, IntegerString y)
    {
      // handle negative numbers
      bool resultIsNegative = x.isNegative != y.isNegative;
      IntegerString result = MultiplyKaratsubaRecursive(
        new IntegerString(x.Digits, false),
        new IntegerString(y.Digits, false)
      );
      result.isNegative = resultIsNegative;
      return (result);
    }

    private static IntegerString MultiplyKaratsubaRecursive(IntegerString x, IntegerString y)
    {
      if (x.Count == 1 && y.Count == 1)
      {
        return (MultiplyKaratsubaBase(x, y));
      }

      int halfCount = (Math.Max(x.Count, y.Count) + 1) / 2; // rounding up

      // prepare sub values
      IntegerString a = GetDigitRange(x, halfCount);
      IntegerString b = GetDigitRange(x, 0, halfCount);
      IntegerString c = GetDigitRange(y, halfCount);
      IntegerString d = GetDigitRange(y, 0, halfCount);

      // recursively solve
      IntegerString ac = MultiplyKaratsuba(a, c);
      IntegerString bd = MultiplyKaratsuba(b, d);
      IntegerString abcd = MultiplyKaratsuba(a + b, c + d);
      IntegerString w = abcd - ac - bd;

      // calculate answer
      ac.DigitShift(halfCount * 2);
      w.DigitShift(halfCount);
      return (ac + w + bd);
    }

    private static IntegerString MultiplyKaratsubaBase(IntegerString x, IntegerString y)
    {
      int product = int.Parse(x.ToString()) * int.Parse(y.ToString());
      return (new IntegerString(product.ToString()));
    }

    private static IntegerString GetDigitRange(IntegerString x, int start, int end = -1)
    {
      List<int> digits = new List<int>();
      if (start < x.Count)
      {
        digits = x.Digits.GetRange(start, Math.Min(x.Count, end < start ? x.Count : end) - start);
      }
      digits.Add(0);
      return (new IntegerString(digits, false));
    }
  }
}

