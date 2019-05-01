/*
Perform multiplication on IntegerStrings using Gradeschool style algorithm.
(Multiply each digit of number A by number B then add all products together)

Algorithmic Analysis:
  MultiplyGradeschool
    O(n * m) where n and m are the number of digits in the operands
*/

using System.Collections.Generic;
using System.Linq;

namespace AlgorithmMath
{
  public partial class IntegerString
  {
    public void MultiplyGradeschool(string s)
    {
      MultiplyGradeschool(new IntegerString(s));
    }

    public void MultiplyGradeschool(IntegerString x)
    {
      SetSelfEqual(MultiplyGradeschool(this, x));
    }

    public static string MultiplyGradeschool(string x, string y)
    {
      return (MultiplyGradeschool(new IntegerString(x), new IntegerString(y)).ToString());
    }

    public static IntegerString MultiplyGradeschool(IntegerString x, IntegerString y)
    {
      List<IntegerString> products = new List<IntegerString>();

      // calculate all products
      for (int i = 0; i < x.Count; i++)
      {
        // pad with zeros
        List<int> lineProduct = Enumerable.Repeat(0, i).ToList();

        int overflow = 0;
        for (int j = 0; j < y.Count; j++)
        {
          int singleProduct = x[i] * y[j] + overflow;
          lineProduct.Add(singleProduct % 10);
          overflow = singleProduct / 10;
        }
        if (0 != overflow)
        {
          lineProduct.Add(overflow);
        }


        products.Add(new IntegerString(lineProduct, false));
      }

      // sum all products
      IntegerString productSum = products.Aggregate((a, b) => a + b);
      productSum.isNegative = x.isNegative != y.isNegative;
      return (productSum);
    }
  }
}

