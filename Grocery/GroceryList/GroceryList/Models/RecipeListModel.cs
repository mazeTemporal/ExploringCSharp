using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GroceryList.Models
{
  public class RecipeListModel
  {
    public List<string> SelectedRecipes { get; set; } = new List<string>();
    public List<SelectListItem> Recipes { get; set; } = new List<SelectListItem>();
  }
}
