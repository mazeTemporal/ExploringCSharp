using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GroceryList.Models
{
  public class RecipeModel
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public List<IngredientModel> Ingredients { get; set; }

    public string PDF { get; set; }
  }
}
