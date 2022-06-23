using System;
using System.Collections.Generic;
using allSpice.Models;
using allSpice.Repositories;

namespace allSpice.Services
{
  public class IngredientsService
  {
    private readonly IngredientsRepository _repo;
    public IngredientsService(IngredientsRepository repo)
    {
      _repo = repo;
    }
    internal List<Ingredient> GetIngredientsByRecipe(int recipeId)
    {
      List<Ingredient> ingredients = _repo.GetIngredientsByRecipe(recipeId);
      return ingredients;
    }

    internal Ingredient GetIngredient(int id)
    {
      Ingredient ingredient = _repo.GetIngredient(id);
      if (ingredient.GetType() != null)
      {
        return ingredient;
      }
      else
      {
        throw new Exception("Ingredient not found");
      }
    }

    internal Ingredient Create(Ingredient ingyData)
    {
      Ingredient newIng = _repo.Create(ingyData);
      return newIng;
    }
    internal List<Ingredient> Edit(List<Ingredient> edits, string userId)
    {
      foreach (Ingredient edit in edits)
      {
        Ingredient original = GetIngredient(edit.Id);
        if (original == null)
        {
          throw new Exception("We couldn't find that Ingredient in our database.");
        }
        if (original.CreatorId != userId)
        {
          throw new Exception("You are not authorized to edit this.");
        }
        original.Name = edit.Name ?? original.Name;
        original.Quantity = edit.Quantity ?? original.Quantity;
      }
      return _repo.Edit(edits);
    }

    internal void Delete(int id, string userId)
    {
      Ingredient found = GetIngredient(id);
      if (found.CreatorId != userId)
      {
        throw new Exception("You are not authorized to delete this.");
      }
      _repo.Delete(id);
    }

  }
}