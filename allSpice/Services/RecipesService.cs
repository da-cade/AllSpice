using System;
using System.Collections.Generic;
using allSpice.Models;
using allSpice.Repositories;

namespace allSpice.Services
{
  public class RecipesService
  {
    private readonly RecipesRepository _repo;

    public RecipesService(RecipesRepository repo)
    {
      _repo = repo;
    }

    internal List<Recipe> GetAllRecipes()
    {
      List<Recipe> recipes = _repo.GetAllRecipes();
      return recipes;
    }

    internal List<Recipe> GetRecipesByCategory(string category)
    {
      List<Recipe> recipes = _repo.GetRecipesByCategory(category);
      return recipes;
    }

    internal Recipe GetRecipe(int id)
    {
      Recipe recipe = _repo.GetRecipe(id);
      return recipe;
    }

    internal Recipe Create(Recipe recData)
    {
      Recipe newRecipe = _repo.Create(recData);
      return newRecipe;
    }

    internal Recipe Edit(int id, Recipe update)
    {
      Recipe original = GetRecipe(id);
      if (original.CreatorId != update.CreatorId)
      {
        throw new Exception("You are not allowed to modify this data");
      }

      original.Picture = update.Picture ?? original.Picture;
      original.Title = update.Title ?? original.Title;
      original.Subtitle = update.Subtitle ?? original.Subtitle;
      original.Category = update.Category ?? original.Category;

      _repo.Edit(id, original);
      return original;
    }

    internal void Delete(int recipeId, string userId)
    {
      Recipe found = GetRecipe(recipeId);
      if (found.CreatorId != userId)
      {
        throw new Exception("You are not authorized to delete this.");
      }
      _repo.Delete(recipeId);
    }
  }
}