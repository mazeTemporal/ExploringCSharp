// https://www.codewars.com/kata/meeting/train/csharp

/*
John has invited people. His list is:

s = "Fred:Corwill;Wilfred:Corwill;Barney:Tornbull;Betty:Tornbull;Bjon:Tornbull;Raphael:Corwill;Alfred:Corwill";
Could you make a program that

makes this string uppercase
gives it sorted in alphabetical order by last name. When the last names are the same, sort them by first name. Last name and first name of a guest come in the result between parentheses separated by a comma. So the result of function meeting(s) will be:
"(CORWILL, ALFRED)(CORWILL, FRED)(CORWILL, RAPHAEL)(CORWILL, WILFRED)(TORNBULL, BARNEY)(TORNBULL, BETTY)(TORNBULL, BJON)"
It can happen that in two distinct families with the same family name two people have the same first name too.
*/

using System;
using System.Linq;
using System.Collections.Generic;

public class JohnMeeting
{
  public static string Meeting(string s) {
    NameList myNameList = new NameList(s);
    myNameList.ToUpper();
    myNameList.Sort();
    return(myNameList.ToString());
  }

  class NameList{
    private List<Name> names;

    public NameList(string s){
      names = new List<Name>();

      string[] entries = s.Split(';');
      foreach (string entry in entries)
      {
        string[] firstLast = entry.Split(':');
        AddName(firstLast[0], firstLast[1]);
      }
    }

    public void AddName(string firstName, string lastName){
      names.Add(new Name(firstName, lastName));
    }

    public void ToUpper(){
      names.ForEach(x => x.ToUpper());
    }

    public void Sort(){
      names = names.OrderBy(x => x.last)
        .ThenBy(x => x.first)
        .ToList();
    }

    public override string ToString(){
      return(
        names.Select(x => x.ToString())
          .Aggregate((a, b) => a + b)
      );
    }
  }
  
  class Name
  {
    public string first;
    public string last;

    public Name(string firstName, string lastName)
    {
      first = firstName;
      last = lastName;
    }

    public void ToUpper(){
      first = first.ToUpper();
      last = last.ToUpper();
    }

    public override string ToString(){
      return(string.Format("({0}, {1})", last, first));
    }
  }
}

