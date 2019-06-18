using System;
namespace GroceryList.Models
{
  public class UnitConversionModel
  {
    public enum UnitSystem
    {
      US,
      Metric
    }

    public enum DbUnitType
    {
      Item,
      Gram,
      Milliliter
    }

    public enum DisplayUnitType
    {
      Item,
      Gram,
      Milliliter,
      Liter,
      Ounce,
      Pound,
      Fluid_Ounce,
      Teaspoon,
      Tablespoon,
      Cup,
      Pint,
      Quart,
      Gallon
    }

    public readonly DbUnitType DbUnit;
    public readonly UnitSystem System;
    public readonly DisplayUnitType DisplayUnit;
    public readonly double ConversionRate;
    public readonly double RangeMin;

    public UnitConversionModel(DbUnitType dbUnit, UnitSystem system, DisplayUnitType displayUnit,
      double conversionRate, double rangeMin)
    {
      DbUnit = dbUnit;
      System = system;
      DisplayUnit = displayUnit;
      ConversionRate = conversionRate;
      RangeMin = rangeMin;
    }
  }
}
