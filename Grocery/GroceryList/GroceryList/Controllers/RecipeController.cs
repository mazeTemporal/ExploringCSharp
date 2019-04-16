using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroceryList.Models;

namespace GroceryList.Controllers
{
  public class RecipeController : Controller
  {
    //!!! sample data for mock up testing
    public List<RecipeModel> recipes = new List<RecipeModel>
      {
        new RecipeModel
        {
          Id = 15,
          Name = "Spaghetti",
          Ingredients = new List<IngredientModel>
          {
            new IngredientModel
            {
              Name = "Spaghetti Noodle",
              Amount = 0.5,
              Unit = IngredientModel.UnitType.Pound,
              Category = "Pasta"
            }
          },
          PDF = "12345.pdf"
        },
        new RecipeModel
        {
          Id = 20,
          Name = "French Toast",
          Ingredients = new List<IngredientModel>
          {
            new IngredientModel
            {
              Name = "Flour",
              Amount = 0.5,
              Unit = IngredientModel.UnitType.Pound,
              Category = "Baking"
            },
            new IngredientModel
            {
              Name = "Egg",
              Amount = 1,
              Unit = IngredientModel.UnitType.Item,
              Category = "Produce"
            }
          },
          PDF = "67890.pdf"
        },
        new RecipeModel{
          Id = 48,
          Name = "Burrito",
          Ingredients = new List<IngredientModel>
          {
            new IngredientModel
            {
              Name = "Tortilla",
              Amount = 3,
              Unit = IngredientModel.UnitType.Item,
              Category = "Foreign"
            },
            new IngredientModel
            {
              Name = "Ground Beef",
              Amount = 1,
              Unit = IngredientModel.UnitType.Pound,
              Category = "Meat"
            },
            new IngredientModel
            {
              Name = "Grated Chedar Cheese",
              Amount = 0.25,
              Unit = IngredientModel.UnitType.Pound,
              Category = "Produce"
            }
          },
          PDF = "789456.pdf"
        }
      };

    public ActionResult Index()
    {
      //!!! database lookup
      return View(recipes);
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
        //!!! save to database
        return RedirectToAction("Show", new { name = recipe.Name });
      }
    }

    public ActionResult Show(string name)
    {
      //!!! should be database lookup
      RecipeModel recipe = recipes.FirstOrDefault(x => x.Name == name);
      if (null == recipe || !ModelState.IsValid)
      {
        return RedirectToAction("Create");
      }
      return View(recipe);
    }

    [HttpGet]
    public ActionResult Edit(string name)
    {
      //!!! should be database lookup
      RecipeModel recipe = recipes.FirstOrDefault(x => x.Name == name);
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
        //!!! save to database
        return RedirectToAction("Show", new { name = recipe.Name });
      }
    }

    [HttpPost]
    public ActionResult Delete(string name)
    {
      //!!! should be database action
      return RedirectToAction("Index");
    }
  }
}
