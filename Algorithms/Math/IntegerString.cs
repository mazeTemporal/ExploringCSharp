/*
Class to perform numeric operations on arbitrarily large integers by representing them as base 10 strings.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AlgorithmMath
{
  public partial class IntegerString : 
    IComparer<IntegerString>, IComparable<IntegerString>,
    IEquatable<IntegerString>, IEqualityComparer<IntegerString>
  {
    private bool isNegative = false;

    // internally stored as reversed list of digits
    private List<int> _digits;
    private List<int> Digits
    {
      get { return (new List<int>(_digits)); }
      set { _digits = new List<int>(value); }
    }

    private int this[int i]
    {
      get
      {
        return (Digits[i]);
      }
    }

    public int Count { get { return (Digits.Count); } }
    public int Length { get { return (Count); } }

    public IntegerString(string s)
    {
      Regex validInteger = new Regex(@"^-?\d+$", RegexOptions.Compiled);
      if (!validInteger.IsMatch(s))
      {
        throw new ArgumentException("Invalid integer string: " + s);
      }
      if ('-' == s[0])
      {
        isNegative = true;
      }
      Digits = s.ToCharArray().Skip(isNegative ? 1 : 0).Reverse().Select(x => x - '0').ToList();
    }

    public IntegerString(IntegerString s)
    {
      isNegative = s.isNegative;
      Digits = s.Digits;
    }

    private IntegerString(List<int> digits, bool isNegative)
    {
      this.Digits = digits;
      this.isNegative = isNegative;
    }

    public override string ToString()
    {
      return (
        (isNegative ? "-" : "") +
        Digits.Select(x => x.ToString())
          .Reverse()
          .Aggregate((a, b) => a + b)
      );
    }

    private void SetSelfEqual(IntegerString s)
    {
      isNegative = s.isNegative;
      Digits = s.Digits;
    }

    private static IntegerString GetInverse(IntegerString s)
    {
      return (new IntegerString(s.Digits, !s.isNegative));
    }

    // IComparable
    public int CompareTo(IntegerString x)
    {
      return (CompareStatic(this, x));
    }

    // IComparer
    public int Compare(IntegerString x, IntegerString y)
    {
      return (CompareStatic(x, y));
    }

    public static int CompareStatic(IntegerString x, IntegerString y)
    {
      if (x.isNegative != y.isNegative)
      {
        return (x.isNegative ? -1 : 1);
      }
      int absoluteCompare = CompareAbsolute(x, y);
      return (x.isNegative ? -absoluteCompare : absoluteCompare);
    }

    public static int CompareAbsolute(IntegerString x, IntegerString y)
    {
      if (x.Digits.Count != y.Digits.Count)
      {
        return (x.Digits.Count - y.Digits.Count);
      }
      for (int i = x.Digits.Count - 1; i >= 0; i--)
      {
        if (x[i] != y[i])
        {
          return (x[i] - y[i]);
        }
      }
      return (0);
    }

    // override for ==
    public override int GetHashCode()
    {
      return GetHashCode(this);
    }

    // IEqualityComparer
    public int GetHashCode(IntegerString x)
    {
      return x.ToString().GetHashCode();
    }

    // override for ==
    public override bool Equals(object obj)
    {
      IntegerString x = obj as IntegerString;
      return (null != x && this == x);
    }

    // IEquatable
    public bool Equals(IntegerString x)
    {
      return (this == x);
    }

    // IEqualityComparer
    public bool Equals(IntegerString x, IntegerString y)
    {
      return (x == y);
    }

    public static bool operator ==(IntegerString x, IntegerString y)
    {
      return (CompareStatic(x, y) == 0);
    }

    public static bool operator !=(IntegerString x, IntegerString y)
    {
      return (CompareStatic(x, y) != 0);
    }

    public static bool operator >(IntegerString x, IntegerString y)
    {
      return (CompareStatic(x, y) > 0);
    }

    public static bool operator <(IntegerString x, IntegerString y)
    {
      return (CompareStatic(x, y) < 0);
    }

    public static bool operator >=(IntegerString x, IntegerString y)
    {
      return (CompareStatic(x, y) >= 0);
    }

    public static bool operator <=(IntegerString x, IntegerString y)
    {
      return (CompareStatic(x, y) <= 0);
    }
  }
}

