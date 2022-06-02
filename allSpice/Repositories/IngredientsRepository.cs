using System.Collections.Generic;
using System.Data;
using System.Linq;
using allSpice.Models;
using Dapper;

namespace allSpice.Repositories
{
  public class IngredientsRepository
  {
    private readonly IDbConnection _db;

    public IngredientsRepository(IDbConnection db)
    {
      _db = db;
    }
    internal List<Ingredient> GetIngredientsByRecipe(int recipeId)
    {
      string sql = @"
      SELECT * FROM ingredients i
      WHERE i.recipeId = @recipeId;
      ";
      return _db.Query<Ingredient>(sql, new { recipeId }).ToList();
    }

    internal Ingredient GetIngredient(int id)
    {
      string sql = @"
      SELECT * FROM ingredients i
      WHERE i.id = @id
      ";
      return _db.QueryFirstOrDefault<Ingredient>(sql, new { id });
    }

    internal Ingredient Create(Ingredient ingData)
    {
      string sql = @"
        INSERT INTO ingredients
        (name, quantity, recipeId, creatorId)
        VALUES
        (@Name, @Quantity, @RecipeId, @CreatorId);
        SELECT LAST_INSERT_ID();
        ";
      ingData.Id = _db.ExecuteScalar<int>(sql, ingData);
      return ingData;
    }
    internal Ingredient Edit(Ingredient original)
    {
      string sql = @"
      UPDATE ingredients
      SET
        name = @name,
        quantity = @quantity
      WHERE id = @Id
      ";
      _db.Execute(sql, original);
      return original;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM ingredients WHERE id = @Id LIMIT 1";
      _db.Execute(sql, new { id });
    }

  }
}