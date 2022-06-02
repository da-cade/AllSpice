using System;
using System.Collections.Generic;
using allSpice.Models;
using allSpice.Repositories;

namespace allSpice.Services
{
  public class StepsService
  {
    private readonly StepsRepository _repo;
    public StepsService(StepsRepository repo)
    {
      _repo = repo;
    }
    internal List<Step> GetStepsByRecipe(int recipeId)
    {
      List<Step> steps = _repo.GetStepsByRecipe(recipeId);
      return steps;
    }

    internal Step GetStep(int id)
    {
      Step step = _repo.GetStep(id);
      return step;
    }

    internal Step Create(Step stepData)
    {
      Step newStep = _repo.Create(stepData);
      return newStep;
    }
    internal Step Edit(Step update, string userId)
    {
      Step original = GetStep(update.Id);
      if (original == null)
      {
        throw new Exception("We couldn't find that Step in our database.");
      }
      if (original.CreatorId != userId)
      {
        throw new Exception("You are not authorized to edit this.");
      }
      original.Body = update.Body ?? original.Body;
      original.Position = update.Position;
      return _repo.Edit(original);
    }
    internal void Delete(int id, string userId)
    {
      Step found = GetStep(id);
      if (found.CreatorId != userId)
      {
        throw new Exception("You are not authorized to delete this.");
      }
      _repo.Delete(id);
    }

  }
}