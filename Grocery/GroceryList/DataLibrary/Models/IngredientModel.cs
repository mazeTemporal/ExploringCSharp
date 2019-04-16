namespace DataLibrary.Models
{
  public class IngredientModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryModel Category { get; set; }
  }
}
