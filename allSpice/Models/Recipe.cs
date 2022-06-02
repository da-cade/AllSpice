using System;

namespace allSpice.Models
{
  public class Recipe
  {
    public int Id { get; set; }
    public string Picture { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Category { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
  }
}