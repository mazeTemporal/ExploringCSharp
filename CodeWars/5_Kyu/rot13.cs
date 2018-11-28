// https://www.codewars.com/kata/530e15517bc88ac656000716/train/csharp

/*
ROT13 is a simple letter substitution cipher that replaces a letter with the letter 13 letters after it in the alphabet. ROT13 is an example of the Caesar cipher.

Create a function that takes a string and returns the string ciphered with Rot13. If there are numbers or special characters included in the string, they should be returned as they are. Only letters from the latin/english alphabet should be shifted, like in the original Rot13 "implementation".

Please note that using "encode" in Python is considered cheating.
*/

using System;
using System.Linq;
using System.Text.RegularExpressions;

public class Kata
{
  public static string Rot13(string message)
  {
    return (new string(
      message.ToCharArray()
             .Select(x => CaesarCipher(x, 13))
             .ToArray()
    ));
  }

  public static char CaesarCipher(char c, int rotation)
  {
    Regex letterCheck = new Regex("[a-zA-Z]");
    // handle letters
    if (letterCheck.IsMatch(c.ToString()))
    {
      char startChar = Char.IsUpper(c) ? 'A' : 'a';
      c = (char)((c - startChar + rotation) % 26 + startChar);
    }
    // handle nonletters
    return (c);
  }
}

