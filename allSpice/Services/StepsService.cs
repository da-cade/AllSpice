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

    internal List<Step> EditMany(List<Step> edits, string userId)
    {
      foreach (Step edit in edits)
      {
        Step original = GetStep(edit.Id);
        if (original == null)
        {
          throw new Exception("We couldn't find that Step in our database.");
        }
        if (original.CreatorId != userId)
        {
          throw new Exception("You are not authorized to edit this.");
        }
        original.Body = edit.Body ?? original.Body;
        original.Position = edit.Position;
      }
      return _repo.Edit(edits);
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