using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLibrary.DataAccess;
using GroceryList.Models;
using GroceryList.Translate;

namespace GroceryList.Controllers
{
  public class RecipeController : Controller
  {
    public ActionResult Index()
    {
      RecipeListModel recipeList = new RecipeListModel()
      {
        Recipes = RecipeProcessor.ReadAllRecipes()
          .Select(ModelTranslator.TranslateRecipeModel)
          .Select(x => new SelectListItem { Text = x.Name, Value = x.Name })
          .ToList()
      };
      return View(recipeList);
    }

    [HttpGet]
    public ActionResult Create()
    {
      RecipeModel recipe = new RecipeModel();
      return View(recipe);
    }

    [HttpPost]
    public ActionResult Create(RecipeModel recipe)
    {
      // verify model
      if (!ModelState.IsValid)
      {
        return View();
      }
      else
      {
        RecipeProcessor.CreateRecipe(ModelTranslator.TranslateRecipeModel(recipe));
        return RedirectToAction("Show", new { name = recipe.Name });
      }
    }

    public ActionResult Show(string name)
    {
      var yolo = RecipeProcessor.ReadRecipe(name);
      RecipeModel recipe = ModelTranslator.TranslateRecipeModel(RecipeProcessor.ReadRecipe(name));
      if (null == recipe || !ModelState.IsValid)
      {
        return RedirectToAction("Create");
      }
      return View(recipe);
    }

    [HttpGet]
    public ActionResult Edit(string name)
    {
      RecipeModel recipe = ModelTranslator.TranslateRecipeModel(RecipeProcessor.ReadRecipe(name));
      if (null == recipe || !ModelState.IsValid)
      {
        return RedirectToAction("Create");
      }
      return View(recipe);
    }

    [HttpPost]
    public ActionResult Edit(RecipeModel recipe)
    {
      // verify model
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Edit", new { name = recipe.Name });
      }
      else
      {
        RecipeProcessor.UpdateRecipe(ModelTranslator.TranslateRecipeModel(recipe));
        return RedirectToAction("Show", new { name = recipe.Name });
      }
    }

    [HttpPost]
    public ActionResult Delete(string name)
    {
      //!!! should be database action
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult ShoppingList(RecipeListModel recipeList)
    {
      List<IngredientModel> ingredients = recipeList.SelectedRecipes
        .Select(RecipeProcessor.ReadRecipe)
        .Select(ModelTranslator.TranslateRecipeModel)
        .Select(x => x.Ingredients)
        .Aggregate((a, b) =>
        { 
          a.AddRange(b);
          return a;
        });
      return View(ingredients);
    }
  }
}
