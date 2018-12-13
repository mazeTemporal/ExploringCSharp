// https://www.codewars.com/kata/52c4dd683bfd3b434c000292/train/csharp

/*
"7777...8?!??!", exclaimed Bob, "I missed it again! Argh!" Every time there's an interesting number coming up, he notices and then promptly forgets. Who doesn't like catching those one-off interesting mileage numbers?

Let's make it so Bob never misses another interesting number. We've hacked into his car's computer, and we have a box hooked up that reads mileage numbers. We've got a box glued to his dash that lights up yellow or green depending on whether it receives a 1 or a 2 (respectively).

It's up to you, intrepid warrior, to glue the parts together. Write the function that parses the mileage number input, and returns a 2 if the number is "interesting" (see below), a 1 if an interesting number occurs within the next two miles, or a 0 if the number is not interesting.

Note: In Haskell, we use No, Almost and Yes instead of 0, 1 and 2.

"Interesting" Numbers
Interesting numbers are 3-or-more digit numbers that meet one or more of the following criteria:

Any digit followed by all zeros: 100, 90000
Every digit is the same number: 1111
The digits are sequential, incementing†: 1234
The digits are sequential, decrementing‡: 4321
The digits are a palindrome: 1221 or 73837
The digits match one of the values in the awesomePhrases array
† For incrementing sequences, 0 should come after 9, and not before 1, as in 7890.
‡ For decrementing sequences, 0 should come after 1, and not before 9, as in 3210.

So, you should expect these inputs and outputs:

// "boring" numbers
Kata.IsInteresting(3, new List<int>() { 1337, 256 });    // 0
Kata.IsInteresting(3236, new List<int>() { 1337, 256 });    // 0

// progress as we near an "interesting" number
Kata.IsInteresting(11207, new List<int>() { });   // 0
Kata.IsInteresting(11208, new List<int>() { });   // 0
Kata.IsInteresting(11209, new List<int>() { });   // 0
Kata.IsInteresting(11210, new List<int>() { });   // 0
Kata.IsInteresting(11211, new List<int>() { });   // 0

// nearing a provided "awesome phrase"
Kata.IsInteresting(1335, new List<int>() { 1337, 256 });   // 1
Kata.IsInteresting(1336, new List<int>() { 1337, 256 });   // 1
Kata.IsInteresting(1337, new List<int>() { 1337, 256 });   // 2
Error Checking
A number is only interesting if it is greater than 99!
Input will always be an integer greater than 0, and less than 1,000,000,000.
The awesomePhrases array will always be provided, and will always be an array, but may be empty. (Not everyone thinks numbers spell funny words...)
You should only ever output 0, 1, or 2.

*/

using System.Collections.Generic;
using System.Linq;

public static class Kata
{
  public static int IsInteresting(int number, List<int> awesomePhrases)
  {
    const int MIN_VAL = 100;
    int maxOffset = 2;
    // must be at least minimum value
    if (number + maxOffset < MIN_VAL){
      return (0);
    }
    if (number >= MIN_VAL)
    {
      // check for exact match
      if (IsInterestingOffset(number, awesomePhrases, 0))
      {
        return (2);
      }
    }
    else
    {
      // avoid checking invalid numbers
      maxOffset -= MIN_VAL - number;
      number = MIN_VAL;
    }
    // check for almost match
    if (IsInterestingOffset(number, awesomePhrases, maxOffset))
    {
      return (1);
    }
    // then it is a boring number
    return (0);
  }

  public static bool IsInterestingOffset(int number, List<int> awesomePhrases, int maxOffset = 0)
  {
    return (
      IsAwesomePhrase(number, awesomePhrases, maxOffset) ||
      IsZeroNumber(number, maxOffset) ||
      IsSequential(number, maxOffset) ||
      IsPalindromic(number,  maxOffset)
    );
  }

  // return true if number is within maxOffset below any member of phrases
  private static bool IsAwesomePhrase(int n, List<int> phrases, int maxOffset = 0)
  {
    int nMax = n + maxOffset;
    foreach (int phrase in phrases)
    {
      if (
        phrase == n ||
        (phrase > n && phrase <= nMax)
      )
      {
        return(true);
      }
    }
    return (false);
  }

  private static bool IsZeroNumber(int n, int maxOffset = 0)
  {
    // if n is follewed by all 0,
    //   first digit of n - 1 will change
    // if n is within maxOffset of an all 0 number,
    //   first digit of n + maxOffset will change
    char initial = n.ToString()[0];
    char lower = (n - 1).ToString()[0];
    char upper = (n + maxOffset).ToString()[0];
    return (!(initial == lower && initial == upper));
  }

  private static bool IsSequential(int n, int maxOffset = 0)
  {
    const string ASCEND = "1234567890";
    const string DESCEND = "9876543210";
    for (int i = 0; i <= maxOffset; ++i)
    {
      string s = (n + i).ToString();
      if (ASCEND.Contains(s) || DESCEND.Contains(s))
      {
        return (true);
      }
    }
    return (false);
  }

  private static bool IsPalindromic(int n, int maxOffset = 0)
  {
    for (int i = 0; i <= maxOffset; ++i)
    {
      if (IsPalindrome(n + i))
      {
        return (true);
      }
    }
    return (false);
  }

  private static bool IsPalindrome(int n)
  {
    string s = n.ToString();
    for (int i = 0; i < s.Length / 2; ++i)
    {
      if (s[i] != s[s.Length - i - 1])
      {
        return (false);
      }
    }
    return (true);
  }
}

