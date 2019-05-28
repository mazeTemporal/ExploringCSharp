using System.Data;
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
                ( Name, Category ) VALUES
                ( @IngredientName, ( SELECT Id FROM Category C WHERE C.Category = 'Uncategorized' ) )";
              totalInserts += connection.Execute(sql,
                new { IngredientName = recipeIngredient.Ingredient.Name });

              // join recipe with ingredient
              sql = @"INSERT INTO RecipeIngredient
                ( Recipe, Ingredient, Amount, Unit ) VALUES
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

    public static RecipeModel ReadRecipe(string recipeName)
    {
      // model to be populated with nested data
      RecipeModel outputRecipe = new RecipeModel() { Name = recipeName };

      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        // get recipe
        string sql = @"
          SELECT R.*, RI.*, I.*, U.*, C.*
          FROM Recipe R
          JOIN RecipeIngredient RI ON R.Id = RI.Recipe
          JOIN Unit U ON RI.Unit = U.Id
          JOIN Ingredient I ON RI.Ingredient = I.Id
          JOIN Category C ON I.Category = C.Id
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
            ingredient.Category = category;
            recipeIngredient.Unit = unit;
            recipeIngredient.Ingredient = ingredient;
            outputRecipe.RecipeIngredients.Add(recipeIngredient);
            return (recipe); // not used
          }, new { RecipeName = recipeName });
      }
      return (outputRecipe);
    }

    public static int UpdateRecipe(RecipeModel recipe)
    {
      using (IDbConnection connection = SQLiteDataAccess.GetConnection())
      {
        connection.Open();
        using (IDbTransaction transaction = connection.BeginTransaction())
        {
          string sql;

          try
          {
            // update recipe
            sql = @"UPDATE Recipe SET Name = @RecipeName, PDF = @PDF WHERE RecipeId = @Id";
            int rowsUpdated = connection.Execute(sql,
              new
              {
                RecipeName = recipe.Name,
                PDF = recipe.PDF,
                RecipeId = recipe.Id
              });

            // delete recipe ingredients
            sql = @"DELETE FROM RecipeIngredient RI WHERE RI.Recipe = @RecipeId";
            rowsUpdated += connection.Execute(sql,
              new { RecipeId = recipe.Id });

            foreach (RecipeIngredientModel recipeIngredient in recipe.RecipeIngredients)
            {
              // create ingredients if necessary
              sql = @"INSERT OR IGNORE INTO Ingredient
              ( IngredientName, Category ) VALUES
              ( @Name, ( SELECT Id FROM Category C WHERE C.Category = 'Uncategorized' ) )";
              rowsUpdated += connection.Execute(sql,
                new { IngredientName = recipeIngredient.Ingredient.Name });

              // create recipe ingredients
              sql = @"INSERT INTO RecipeIngredient
                ( Recipe, Ingredient, Amount, Unit ) VALUES
                ( @RecipeId,
                ( SELECT Id FROM Ingredient I WHERE I.Name = @IngredientName ),
                @Amount
                ( SELECT Id FROM Unit U WHERE U.Unit = @Unit ) )";
              rowsUpdated += connection.Execute(sql,
                new
                {
                  RecipeId = recipe.Id,
                  IngredientName = recipeIngredient.Ingredient.Name,
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
            sql = @"DELETE FROM RecipeIngredient RI
              WHERE RI.Recipe =
                ( SELECT Id FROM Recipe R WHERE R.Name = @RecipeName)";
            int rowsDeleted = connection.Execute(sql,
              new { RecipeName = recipeName });

            // delete recipe
            sql = @"DELETE FROM Recipe R WHERE R.Name = @RecipeName";
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
