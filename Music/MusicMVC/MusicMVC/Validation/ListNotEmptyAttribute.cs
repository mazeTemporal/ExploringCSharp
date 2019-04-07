using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicMVC.Validation
{
  public class ListNotEmptyAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value is IList<object> myList)
      {
        if (myList.Count > 0)
        {
          return (ValidationResult.Success);
        }
        return new ValidationResult("List must contain at least one value");
      }
      return new ValidationResult("ListNotEmptyAttribute only valid on IList");
    }
  }
}
