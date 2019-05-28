namespace DataLibrary.Models
{
  public class RecipeIngredientModel
  {
    public IngredientModel Ingredient { get; set; }
    public double Amount { get; set; }
    public UnitModel Unit { get; set; }
  }
}
