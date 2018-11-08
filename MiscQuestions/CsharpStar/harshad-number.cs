// https://www.csharpstar.com/check-harshad-number-niven-number-in-csharp/

/*
Problem Statement:
Write a C# program or function to check Harshad number (or Niven number). Your program should take one positive integer from the user as input and check whether this integer is Harshad number (Niven number) or not.
 

What Is Harshad Number Or Niven Number?
Harshad number or Niven number is a number which is divisible by the sum of its digits. For example,

1) 21 is a Harshad number because it is divisible by the sum of its digits.

21 –> sum of digits –> 2+1 = 3 and 21 is divisible by 3 –> 21/3 = 7.

2) 111 is a Harshad number because it is divisible by the sum of its digits.

111 –> sum of digits –> 1+1+1 = 3 and 111 is divisible by 3 –> 111/3 = 37

3) 153 is a Harshad number. It is divisible by the sum of its digits.

153 –> sum of its digits –> 1+5+3 = 9 and 153 is divisible by 9 –> 153/9 = 17
*/

static bool IsHarshad(int x)
{
  // cannot divide by zero
  // negative numbers do not make sense
  return(x > 0 && 0 == x % SumDigits(x));
}

static int SumDigits(int x)
{
  int sum = 0;
  while (x > 0)
  {
    sum += x % 10;
    x /= 10;
  }
  return(sum);
}

