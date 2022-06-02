using System.Collections.Generic;
using System.Data;
using System.Linq;
using allSpice.Models;
using Dapper;

namespace allSpice.Repositories
{
  public class FavoritesRepository
  {
    private readonly IDbConnection _db;

    public FavoritesRepository(IDbConnection db)
    {
      _db = db;
    }

    // TODO Recipe virtuals
    internal List<Favorite> GetFavoritesByRecipe(int recipeId)
    {
      string sql = @"
      SELECT fav.*, r.*, acct.* 
      FROM favorites fav
      JOIN recipes r ON fav.recipeId = r.id
      JOIN accounts acct ON fav.accountId = acct.id
      WHERE fav.recipeId = @recipeId;
      ";
      return _db.Query<Favorite, Recipe, Account, Favorite>(sql, (favorite, recipe, account) =>
      {
        favorite.Recipe = recipe;
        favorite.Account = account;
        return favorite;
      }, new { recipeId }).ToList();
    }

    // TODO profile virtuals
    internal List<Favorite> GetFavoritesByAccount(string accountId)
    {
      string sql = @"
      SELECT fav.*, r.*, acct.* 
      FROM favorites fav
      JOIN recipes r ON fav.recipeId = r.id
      JOIN accounts acct ON fav.accountId = acct.id
      WHERE fav.accountId = @accountId;
      ";
      return _db.Query<Favorite, Recipe, Account, Favorite>(sql, (favorite, recipe, account) =>
      {
        favorite.Recipe = recipe;
        favorite.Account = account;
        return favorite;
      }, new { accountId }).ToList();
    }

    internal Favorite GetFavorite(int id)
    {
      string sql = @"
      SELECT fav.*, r.*, acct.* 
      FROM favorites fav
      JOIN recipes r ON fav.recipeId = r.id
      JOIN accounts acct ON fav.accountId = acct.id
      WHERE fav.id = @id
      ";
      return _db.Query<Favorite, Recipe, Account, Favorite>(sql, (favorite, recipe, account) =>
      {
        favorite.Recipe = recipe;
        favorite.Account = account;
        return favorite;
      }, new { id }).FirstOrDefault();
    }

    internal Favorite Create(Favorite favData)
    {
      string sql = @"
        INSERT INTO favorites
        (accountId, recipeId, createdAt)
        VALUES
        (@accountId, @RecipeId, @CreatedAt);
        SELECT LAST_INSERT_ID();
        ";
      favData.Id = _db.ExecuteScalar<int>(sql, favData);
      return favData;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM favorites WHERE id = @Id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}