using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GroceryList.Models
{
  public class RecipeModel
  {
    [Required]
    public string Recipe { get; set; }

    public List<IngredientModel> Ingredients;

    public string PDF { get; set; }
  }
}
