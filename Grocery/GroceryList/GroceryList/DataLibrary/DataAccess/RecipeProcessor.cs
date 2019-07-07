using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DataLibrary.Models;

namespace DataLibrary.DataAccess
{
  public static class RecipeProcessor
  {
    public static int CreateRecipe(RecipeModel recipe)
    {
      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        connection.Open();
        using (IDbTransaction transaction = connection.BeginTransaction())
        {
          string sql;

          try
          {
            // create recipe
            sql = @"INSERT INTO Recipe ( Name, PDF ) VALUES ( @RecipeName, @PDF )";
            int totalInserts = connection.Execute(sql,
              new { RecipeName = recipe.Name, PDF = recipe.PDF });

            foreach (RecipeIngredientModel recipeIngredient in recipe.RecipeIngredients)
            {
              // create ingredient
              sql = @"INSERT OR IGNORE INTO Ingredient
                ( Name, CategoryId ) VALUES
                ( @IngredientName, ( SELECT Id FROM Category C WHERE C.Category = 'Uncategorized' ) )";
              totalInserts += connection.Execute(sql,
                new { IngredientName = recipeIngredient.Ingredient.Name });

              // join recipe with ingredient
              sql = @"INSERT INTO RecipeIngredient
                ( RecipeId, IngredientId, Amount, UnitId ) VALUES
                ( ( SELECT Id FROM Recipe R WHERE R.Name = @RecipeName ),
                ( SELECT Id FROM Ingredient I WHERE I.Name = @IngredientName ),
                @Amount,
                ( SELECT Id FROM Unit U WHERE U.Unit = @Unit))";
              totalInserts += connection.Execute(sql,
                new
                {
                  RecipeName = recipe.Name,
                  IngredientName = recipeIngredient.Ingredient.Name,
                  Amount = recipeIngredient.Amount,
                  Unit = recipeIngredient.Unit.Unit
                });
            }

            transaction.Commit();
            return (totalInserts);
          }
          catch
          {
            transaction.Rollback();
            return (0);
          }
        }
      }
    }

    public static List<RecipeModel> ReadAllRecipes()
    {
      Dictionary<string, RecipeModel> output = new Dictionary<string, RecipeModel>();

      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        string sql = @"
          SELECT R.*, RI.*, I.*, U.*, C.*
          FROM Recipe R
          JOIN RecipeIngredient RI ON R.Id = RI.RecipeId
          JOIN Unit U ON RI.UnitId = U.Id
          JOIN Ingredient I ON RI.IngredientId = I.Id
          JOIN Category C ON I.CategoryId = C.Id
        ";

        connection.Query<
          RecipeModel,
          RecipeIngredientModel,
          IngredientModel,
          UnitModel,
          CategoryModel,
          RecipeModel // not used, populating external object
          >(sql, (recipe, recipeIngredient, ingredient, unit, category) =>
          {
            ingredient.Category = category;
            recipeIngredient.Unit = unit;
            recipeIngredient.Ingredient = ingredient;
            if (!output.ContainsKey(recipe.Name))
            {
              recipe.RecipeIngredients = new List<RecipeIngredientModel>();
              output.Add(recipe.Name, recipe);
            }
            output[recipe.Name].RecipeIngredients.Add(recipeIngredient);
            return recipe;
          });

        return output.Values.ToList();
      }
    }

    public static RecipeModel ReadRecipe(string recipeName)
    {
      // model to be populated with nested data
      RecipeModel output = null;

      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        // get recipe
        string sql = @"
          SELECT R.*, RI.*, I.*, U.*, C.*
          FROM Recipe R
          JOIN RecipeIngredient RI ON R.Id = RI.RecipeId
          JOIN Unit U ON RI.UnitId = U.Id
          JOIN Ingredient I ON RI.IngredientId = I.Id
          JOIN Category C ON I.CategoryId = C.Id
          WHERE R.Name = @RecipeName
        ";

        connection.Query<
          RecipeModel,
          RecipeIngredientModel,
          IngredientModel,
          UnitModel,
          CategoryModel,
          RecipeModel // not used, populate external object
          >(sql, (recipe, recipeIngredient, ingredient, unit, category) =>
          {
            if (output == null)
            {
              recipe.RecipeIngredients = new List<RecipeIngredientModel>();
              output = recipe;
            }
            ingredient.Category = category;
            recipeIngredient.Unit = unit;
            recipeIngredient.Ingredient = ingredient;
            output.RecipeIngredients.Add(recipeIngredient);
            return (recipe); // not used
          }, new { RecipeName = recipeName });

        return output;
      }
    }

    public static RecipeModel CombineRecipes(List<string> recipeNames)
    {
      // model to be populated with nested data
      RecipeModel output = null;

      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        // get recipe
        string sql = @"
          SELECT R.*, RI.*, SUM(RI.Amount) AS 'Amount', I.*, U.*, C.*
          FROM Recipe R
          JOIN RecipeIngredient RI ON R.Id = RI.RecipeId
          JOIN Unit U ON RI.UnitId = U.Id
          JOIN Ingredient I ON RI.IngredientId = I.Id
          JOIN Category C ON I.CategoryId = C.Id
          WHERE R.Name in @recipeNames
          GROUP BY RI.IngredientId, RI.UnitId
        ";

        connection.Query<
          RecipeModel,
          RecipeIngredientModel,
          IngredientModel,
          UnitModel,
          CategoryModel,
          RecipeModel // not used, populate external object
          >(sql, (recipe, recipeIngredient, ingredient, unit, category) =>
          {
            if (output == null)
            {
              recipe.RecipeIngredients = new List<RecipeIngredientModel>();
              output = recipe;
            }
            ingredient.Category = category;
            recipeIngredient.Unit = unit;
            recipeIngredient.Ingredient = ingredient;
            output.RecipeIngredients.Add(recipeIngredient);
            return (recipe); // not used
          }, new { recipeNames });

        return output;
      }
    }

    public static int UpdateRecipe(RecipeModel recipe)
    {
      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        connection.Open();
        using (IDbTransaction transaction = connection.BeginTransaction())
        {
          try
          {
            // update recipe
            string sql = @"UPDATE Recipe SET Name = @Name, PDF = @PDF WHERE Id = @Id";
            int rowsUpdated = connection.Execute(sql,
              new {
                Name = recipe.Name,
                PDF = recipe.PDF,
                Id = recipe.Id
              });

            // delete recipe ingredients
            sql = @"DELETE FROM RecipeIngredient WHERE RecipeIngredient.RecipeId = @RecipeId";
            rowsUpdated += connection.Execute(sql,
              new { RecipeId = recipe.Id });

            foreach (RecipeIngredientModel recipeIngredient in recipe.RecipeIngredients)
            {
              // create ingredients if necessary
              sql = @"INSERT OR IGNORE INTO Ingredient
              ( Name, CategoryId ) VALUES
              ( @Name, ( SELECT Id FROM Category C WHERE C.Category = 'Uncategorized' ) )";
              rowsUpdated += connection.Execute(sql,
                new { Name = recipeIngredient.Ingredient.Name });

              // create recipe ingredients
              sql = @"INSERT INTO RecipeIngredient
                ( RecipeId, IngredientId, Amount, UnitId ) VALUES
                ( @RecipeId,
                ( SELECT Id FROM Ingredient I WHERE I.Name = @Name ),
                @Amount,
                ( SELECT Id FROM Unit U WHERE U.Unit = @Unit ) )";
              rowsUpdated += connection.Execute(sql,
                new
                {
                  RecipeId = recipe.Id,
                  Name = recipeIngredient.Ingredient.Name,
                  Amount = recipeIngredient.Amount,
                  Unit = recipeIngredient.Unit.Unit
                });
            }

            transaction.Commit();
            return (rowsUpdated);
          }
          catch
          {
            transaction.Rollback();
            return (0);
          }
        }
      }
    }

    public static int DeleteRecipe(string recipeName)
    {
      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        connection.Open();
        using (IDbTransaction transaction = connection.BeginTransaction())
        {
          string sql;

          try
          {
            // delete recipe ingredients
            sql = @"DELETE FROM RecipeIngredient
              WHERE RecipeIngredient.RecipeId =
                ( SELECT Id FROM Recipe R WHERE R.Name = @RecipeName)";
            int rowsDeleted = connection.Execute(sql,
              new { RecipeName = recipeName });

            // delete recipe
            sql = @"DELETE FROM Recipe WHERE Recipe.Name = @RecipeName";
            rowsDeleted = connection.Execute(sql,
              new { RecipeName = recipeName });

            transaction.Commit();
            return (rowsDeleted);
          }
          catch
          {
            transaction.Rollback();
            return (0);
          }
        }
      }
    }
  }
}
