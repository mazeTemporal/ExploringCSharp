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

    public static RecipeModel TranslateRecipeModel(Models.RecipeModel displayRecipe)
    {
      RecipeModel dbRecipe = new RecipeModel()
      {
        Id = displayRecipe.Id,
        Name = displayRecipe.Name,
        PDF = displayRecipe.PDF,
        RecipeIngredients = new List<RecipeIngredientModel>()
      };

      foreach (Models.IngredientModel displayIngredient in displayRecipe.Ingredients)
      {
        dbRecipe.RecipeIngredients.Add(TranslateIngredientModel(displayIngredient));
      }

      return dbRecipe;
    }

    public static Models.IngredientModel TranslateIngredientModel(RecipeIngredientModel dbIngredient)
    {
      Models.IngredientModel displayIngredient = new Models.IngredientModel
      {
        Id = dbIngredient.Ingredient.Id,
        Name = dbIngredient.Ingredient.Name,
        Amount = dbIngredient.Amount,
        Category = dbIngredient.Ingredient.Category.Category
      };

      displayIngredient.Unit = TranslateUnitModel(dbIngredient.Unit);

      return displayIngredient;
    }

    public static RecipeIngredientModel TranslateIngredientModel(Models.IngredientModel displayIngredient)
    {
      RecipeIngredientModel dbIngredient = new RecipeIngredientModel()
      {
        Ingredient = new IngredientModel(),
        Amount = displayIngredient.Amount,
        Unit = TranslateUnitModel(displayIngredient.Unit)
      };

      dbIngredient.Ingredient.Name = displayIngredient.Name;
      dbIngredient.Ingredient.Category = new CategoryModel
      {
        Category = displayIngredient.Category
      };

      return dbIngredient;
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

    public static UnitModel TranslateUnitModel(Models.IngredientModel.UnitType displayUnit)
    {
      UnitModel dbUnit = new UnitModel();

      switch (displayUnit)
      {
        case Models.IngredientModel.UnitType.Gram:
          dbUnit.Unit = "Gram";
          break;
        case Models.IngredientModel.UnitType.Millileter:
          dbUnit.Unit = "Milliliter";
          break;
        case Models.IngredientModel.UnitType.Item:
          dbUnit.Unit = "Item";
          break;
        default:
          throw new ArgumentException("Unrecognized UnitType Unit");
      }

      return dbUnit;
    }
  }
}