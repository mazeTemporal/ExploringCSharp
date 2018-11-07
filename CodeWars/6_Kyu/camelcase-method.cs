// https://www.codewars.com/kata/camelcase-method/train/csharp

/*
Write simple .camelCase method (camel_case function in PHP, CamelCase in C# or camelCase in Java) for strings. All words must have their first letter capitalized without spaces.

For instance:

camelCase("hello case"); // => "HelloCase"
camelCase("camel case word"); // => "CamelCaseWord"
*/

using System.Linq;

namespace Kata
{
  public static class Problem
  {
    public static string CamelCase(this string str)  
    {
      return (
        str.Split(' ')
         .Select(Capitalize)
         .Aggregate((a, b) => a + b)
      );
    }

    public static string Capitalize(string str)
    {
      return (
        str.Length == 0 ? "" :
        str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower()
      );
    }
  }
}

