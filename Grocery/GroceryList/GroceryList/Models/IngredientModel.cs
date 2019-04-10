using System.ComponentModel.DataAnnotations;

namespace GroceryList.Models
{
  public class IngredientModel
  {
    public enum UnitType
    {
      Gram,
      Millileter,
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

    [Required]
    public string Ingredient { get; set; }

    [Required]
    public double Amount { get; set; }

    [Required]
    public UnitType Unit { get; set; }

    [Required]
    public string Category { get; set; }
  }
}
