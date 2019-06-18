using System.ComponentModel.DataAnnotations;

namespace GroceryList.Models
{
  public class IngredientModel
  {
    public enum UnitType
    {
      Item,
      Gram,
      Milliliter,
      Liter,
      Ounce,
      Pound,
      Fluid_Ounce,
      Teaspoon,
      Tablespoon,
      Cup,
      Pint,
      Quart,
      Gallon
    }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [RegularExpression(@"\d*\.?\d+", ErrorMessage = "Amount must be a number or decimal.")]
    public double Amount { get; set; }

    [Required]
    public UnitType Unit { get; set; }

    public string Category { get; set; }
  }
}
