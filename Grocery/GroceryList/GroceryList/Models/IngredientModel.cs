using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GroceryList.Models
{
  public class IngredientModel
  {
    private bool initialized = false;

    public static readonly List<UnitConversionModel> UnitConversions = new List<UnitConversionModel>
    {
      new UnitConversionModel(UnitConversionModel.DbUnitType.Item, UnitConversionModel.UnitSystem.Metric, UnitConversionModel.DisplayUnitType.Item, 1, 0),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Item, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Item, 1, 0),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Gram, UnitConversionModel.UnitSystem.Metric, UnitConversionModel.DisplayUnitType.Gram, 1, 0),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Gram, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Ounce, 0.03527396195, 0),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Gram, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Pound, 0.00220462262185, 453),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.Metric, UnitConversionModel.DisplayUnitType.Milliliter, 1, 0),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.Metric, UnitConversionModel.DisplayUnitType.Liter, 0.001, 1000),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Fluid_Ounce, 0.0338140227, -1),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Teaspoon, 0.2028841362, 0),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Tablespoon, 0.0676280454, 14),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Cup, 0.004226752838, 236),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Pint, 0.0021133764188652, 473),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Quart, 0.0010566882094326, 946),
      new UnitConversionModel(UnitConversionModel.DbUnitType.Milliliter, UnitConversionModel.UnitSystem.US, UnitConversionModel.DisplayUnitType.Gallon, 0.0002641720523581, 3785)
    };

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    // Amount set by user
    private double _displayAmount;
    [Required]
    [Display(Name = "Amount")]
    [RegularExpression(@"\d*\.?\d+", ErrorMessage = "Amount must be a number or decimal.")]
    public double DisplayAmount
    {
      get { return RoundToEighth(_displayAmount); }
      set
      {
        _displayAmount = value;
        if (initialized)
        {
          UpdateDbAmount();
        }
      }
    }

    // Internal amount used by system
    [Required]
    [System.Web.Mvc.HiddenInput(DisplayValue = false)]
    public double DbAmount { get; set; }

    // Unit set by user
    private UnitConversionModel.DisplayUnitType _displayUnit;
    [Required]
    [Display(Name = "Unit")]
    public UnitConversionModel.DisplayUnitType DisplayUnit
    {
      get { return _displayUnit; }
      set
      {
        _displayUnit = value;
        if (initialized)
        {
          UpdateDbUnit();
          UpdateDbAmount();
        }
      }
    }

    // Internal unit used by system
    [Required]
    [System.Web.Mvc.HiddenInput(DisplayValue = false)]
    public UnitConversionModel.DbUnitType DbUnit { get; set; }

    public string Category { get; set; }

    public IngredientModel() {
      initialized = true;
    }

    public IngredientModel(double dbAmount, UnitConversionModel.DbUnitType dbUnit)
    {
      DbUnit = dbUnit;
      DbAmount = dbAmount;
      UpdateDisplayUnit();
      UpdateDisplayAmount();
      initialized = true;
    }

    private void UpdateDbAmount()
    {
      DbAmount = DisplayAmount / UnitConversions.First(x => x.DisplayUnit == DisplayUnit).ConversionRate;
    }

    private void UpdateDbUnit()
    {
      DbUnit = UnitConversions.First(x => x.DisplayUnit == DisplayUnit).DbUnit;
    }

    private void UpdateDisplayAmount(UnitConversionModel.UnitSystem system = UnitConversionModel.UnitSystem.US)
    {
      DisplayAmount = DbAmount * UnitConversions.First(x => 
        x.DbUnit == DbUnit &&
        x.DisplayUnit == DisplayUnit &&
        x.System == system
      ).ConversionRate;
    }

    private void UpdateDisplayUnit(UnitConversionModel.UnitSystem system = UnitConversionModel.UnitSystem.US)
    {
      DisplayUnit = UnitConversions
        .Where(x => x.DbUnit == DbUnit && x.System == system) // get candidate units
        .Where(x => DbAmount >= x.RangeMin) // avoid inappropriately large units
        .OrderByDescending(x => x.RangeMin) // select largest remaining unit
        .First().DisplayUnit;
    }

    private static double RoundToEighth(double amount) => Math.Round(amount * 8) / 8;
  }
}
