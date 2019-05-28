using System.Collections.Generic;
namespace DataLibrary.Models
{
  public class RecipeModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string PDF { get; set; }
    public List<RecipeIngredientModel> RecipeIngredients { get; set; }
  }
}
