using System;

namespace allSpice.Models
{
  public class Favorite
  {
    public int Id { get; set; }
    public string AccountId { get; set; }
    public int RecipeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Recipe Recipe { get; set; }
    public Profile Account { get; set; }
  }
}