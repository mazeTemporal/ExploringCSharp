using System.Collections.Generic;

namespace GroceryList.Models
{
  public class GroceryListModel
  {
    public Dictionary<string, List<IngredientModel>> IngredientCategories { get; set; } = new Dictionary<string, List<IngredientModel>>();
  }
}
