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
      Models.IngredientModel displayIngredient = new Models.IngredientModel(dbIngredient.Amount, TranslateUnitModel(dbIngredient.Unit))
      {
        Id = dbIngredient.Ingredient.Id,
        Name = dbIngredient.Ingredient.Name,
        Category = dbIngredient.Ingredient.Category.Category
      };

      return displayIngredient;
    }

    public static RecipeIngredientModel TranslateIngredientModel(Models.IngredientModel displayIngredient)
    {
      CategoryModel category = new CategoryModel
      {
        Category = displayIngredient.Category
      };

      IngredientModel ingredient = new IngredientModel()
      {
        Name = displayIngredient.Name,
        Category = category
      };

      RecipeIngredientModel dbIngredient = new RecipeIngredientModel()
      {
        Ingredient = ingredient,
        Amount = displayIngredient.DbAmount,
        Unit = TranslateUnitModel(displayIngredient.DbUnit)
      };

      return dbIngredient;
    }

    public static Models.UnitConversionModel.DbUnitType TranslateUnitModel(UnitModel dbUnit)
    {
      switch (dbUnit.Unit)
      {
        case "Gram":
          return Models.UnitConversionModel.DbUnitType.Gram;
        case "Milliliter":
          return Models.UnitConversionModel.DbUnitType.Milliliter;
        case "Item":
          return Models.UnitConversionModel.DbUnitType.Item;
        default:
          throw new ArgumentException($"Unrecognized DbUnitType: {dbUnit.Unit}");
      }
    }

    public static UnitModel TranslateUnitModel(Models.UnitConversionModel.DbUnitType displayUnit)
    {
      UnitModel dbUnit = new UnitModel();

      switch (displayUnit)
      {
        case Models.UnitConversionModel.DbUnitType.Gram:
          dbUnit.Unit = "Gram";
          break;
        case Models.UnitConversionModel.DbUnitType.Milliliter:
          dbUnit.Unit = "Milliliter";
          break;
        case Models.UnitConversionModel.DbUnitType.Item:
          dbUnit.Unit = "Item";
          break;
        default:
          throw new ArgumentException("Unrecognized UnitType Unit");
      }

      return dbUnit;
    }

    public static Models.GroceryListModel RecipeToGroceryList(Models.RecipeModel recipe)
    {
      Models.GroceryListModel groceryList = new Models.GroceryListModel();
      foreach (Models.IngredientModel ingredient in recipe.Ingredients)
      {
        if (!groceryList.IngredientCategories.ContainsKey(ingredient.Category))
        {
          groceryList.IngredientCategories[ingredient.Category] = new List<Models.IngredientModel>();
        }
        groceryList.IngredientCategories[ingredient.Category].Add(ingredient);
      }
      return groceryList;
    }
  }
}