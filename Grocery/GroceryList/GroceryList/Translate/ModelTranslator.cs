using System;
using System.Collections.Generic;
using DataLibrary.Models;

namespace GroceryList.Translate
{
  public static class ModelTranslator
  {
    public static Models.RecipeModel TranslateRecipeModel(RecipeModel dbRecipe)
    {
      Models.RecipeModel displayRecipe = new Models.RecipeModel
      {
        Id = dbRecipe.Id,
        Name = dbRecipe.Name,
        PDF = dbRecipe.PDF,
        Ingredients = new List<Models.IngredientModel>()
      };
      foreach (RecipeIngredientModel dbIngredient in dbRecipe.RecipeIngredients)
      {
        displayRecipe.Ingredients.Add(TranslateIngredientModel(dbIngredient));
      }
      return displayRecipe;
    }

    public static Models.IngredientModel TranslateIngredientModel(RecipeIngredientModel dbIngredient)
    {
      Models.IngredientModel displayIngredient = new Models.IngredientModel
      {
        Id = dbIngredient.Ingredient.Id,
        Name = dbIngredient.Ingredient.Name,
        Amount = dbIngredient.Amount
      };

      displayIngredient.Unit = TranslateUnitModel(dbIngredient.Unit);

      return displayIngredient;
    }

    public static Models.IngredientModel.UnitType TranslateUnitModel(UnitModel dbUnit)
    {
      switch (dbUnit.Unit)
      {
        case "Gram":
          return Models.IngredientModel.UnitType.Gram;
        case "Milliliter":
          return Models.IngredientModel.UnitType.Millileter;
        case "Item":
          return Models.IngredientModel.UnitType.Item;
        default:
          throw new ArgumentException("Unrecognized UnitModel Unit");
      }
    }
  }
}