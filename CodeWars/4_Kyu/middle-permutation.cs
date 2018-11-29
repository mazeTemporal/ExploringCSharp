// https://www.codewars.com/kata/58ad317d1541651a740000c5/train/csharp

/*
You are given a string s. Every letter in s appears once.

Consider all strings formed by rearranging the letters in s. After ordering these strings in dictionary order, return the middle term. (If the sequence has a even length n, define its middle term to be the (n/2)th term.)

Example
For s = "abc", the result should be "bac".

The permutations in order are:
"abc", "acb", "bac", "bca", "cab", "cba"
So, The middle term is "bac".

Input/Output
[input] string s
	unique letters (2 <= length <= 26)
[output] a string
	middle permutation.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace myjinxin
{
  public class Kata
  {
    public string MiddlePermutation(string s)
    {
      // ensure alphabetical order
      s = SortString(s);
      // only calculate factorials once
      List<BigInteger> factorials = GetFactorials(s.Length);
      // determine which permutation to construct
      BigInteger target = (factorials[factorials.Count - 1] - 1) / 2;
      // build permutation
      string permutation = "";
      for (int i = s.Length; i > 0; --i)
      {
        int charIndex = (int)(target / factorials[i - 1]);
        target %= factorials[i - 1];
        permutation += s[charIndex];
        s = s.Substring(0, charIndex) + s.Substring(charIndex + 1);
      }
      return (permutation);
    }

    // generate list of factorials from 0 to x
    public static List<BigInteger> GetFactorials(int x)
    {
      List<BigInteger> factorials = new List<BigInteger> {1}; // manually insert 0!
      for (int i = 1; i <= x; ++i)
      {
        factorials.Add(factorials[i - 1] * i);
      }
      return (factorials);
    }

    // sort chars in string
    public static string SortString(string s)
    {
      return(string.Join("", s.ToCharArray().OrderBy(x => x)));
    }
  }
}

