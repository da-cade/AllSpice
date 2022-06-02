using System;
using System.Collections.Generic;
using allSpice.Models;
using allSpice.Repositories;

namespace allSpice.Services
{
  public class FavoritesService
  {
    private readonly FavoritesRepository _repo;
    public FavoritesService(FavoritesRepository repo)
    {
      _repo = repo;
    }
    internal List<Favorite> GetFavoritesByRecipe(int recipeId)
    {
      List<Favorite> favorites = _repo.GetFavoritesByRecipe(recipeId);
      return favorites;
    }
    internal List<Favorite> GetFavoritesByAccount(string userId)
    {
      List<Favorite> favorites = _repo.GetFavoritesByAccount(userId);
      return favorites;
    }
    internal Favorite GetFavorite(int id, string accountId)
    {
      Favorite favorite = _repo.GetFavorite(id);
      if (accountId != favorite.AccountId)
      {
        throw new Exception("You do not have permission to access this.");
      }
      return favorite;
    }
    internal Favorite Create(Favorite favData)
    {
      Favorite newFav = _repo.Create(favData);
      return newFav;
    }

    internal void Remove(int id, string userId)
    {
      Favorite found = GetFavorite(id, userId);
      if (found.AccountId != userId)
      {
        throw new Exception("Error - You are not signed in to the right account.");
      }
      _repo.Delete(id);
    }
  }
}
