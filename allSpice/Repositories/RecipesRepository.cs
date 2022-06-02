using System.Collections.Generic;
using System.Data;
using System.Linq;
using allSpice.Models;
using Dapper;

namespace allSpice.Repositories
{
  public class RecipesRepository
  {
    private readonly IDbConnection _db;

    public RecipesRepository(IDbConnection db)
    {
      _db = db;
    }

    internal List<Recipe> GetAllRecipes()
    {
      string sql = @"
      SELECT r.*, acct.*
      FROM 
      recipes r
      JOIN accounts acct ON r.creatorId = acct.id
      ";
      return _db.Query<Recipe, Account, Recipe>(sql, (recipe, account) =>
      {
        recipe.Creator = account;
        return recipe;
      }).ToList();
    }

    internal List<Recipe> GetRecipesByCategory(string category)
    {
      string sql = @"
      SELECT r.*, acct.*
      FROM 
      recipes r
      JOIN accounts acct ON r.creatorId = acct.id 
      WHERE r.category = @category;
      ";
      return _db.Query<Recipe, Account, Recipe>(sql, (recipe, account) =>
      {
        recipe.Creator = account;
        return recipe;
      }).ToList();
    }

    internal Recipe GetRecipe(int id)
    {
      string sql = @"
      SELECT r.*, acct.*
      FROM 
      recipes r
      JOIN accounts acct ON r.creatorId = acct.id
      WHERE r.id = @id
      ";
      return _db.Query<Recipe, Account, Recipe>(sql, (recipe, account) =>
      {
        recipe.Creator = account;
        return recipe;
      }, new { id }).FirstOrDefault();
    }

    internal Recipe Create(Recipe recData)
    {
      string sql = @"
        INSERT INTO recipes
        (picture, title, subtitle, category, creatorId)
        VALUES
        (@Picture, @Title, @Subtitle, @Category, @CreatorId);
        SELECT LAST_INSERT_ID();
        ";
      recData.Id = _db.ExecuteScalar<int>(sql, recData);
      return recData;
    }

    internal Recipe Edit(int id, Recipe original)
    {
      string sql = @"
        UPDATE recipes
        SET
          picture = @picture,
          title = @title,
          subtitle = @subtitle,
          category = @category
        WHERE id = @Id
        ";
      _db.Execute(sql, original);
      return original;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM recipes WHERE id = @Id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}