using System.Collections.Generic;
using System.Data;
using System.Linq;
using allSpice.Models;
using Dapper;

namespace allSpice.Repositories
{
  public class StepsRepository
  {
    private readonly IDbConnection _db;
    public StepsRepository(IDbConnection db)
    {
      _db = db;
    }
    internal List<Step> GetStepsByRecipe(int recipeId)
    {
      string sql = @"
      SELECT * FROM steps step
      WHERE step.recipeId = @recipeId;
      ";
      return _db.Query<Step>(sql, new { recipeId }).ToList();
    }

    internal Step GetStep(int id)
    {
      string sql = @"
      SELECT * FROM steps step
      WHERE step.id = @id
      ";
      return _db.QueryFirstOrDefault<Step>(sql, new { id });
    }

    internal Step Create(Step stepData)
    {
      string sql = @"
      INSERT INTO steps
      (position, body, recipeId, creatorId)
      VALUES
      (@Position, @Body, @RecipeId, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";
      stepData.Id = _db.ExecuteScalar<int>(sql, stepData);
      return stepData;
    }
    internal Step Edit(Step original)
    {
      string sql = @"
      UPDATE steps
      SET
        body = @body,
        position = @position
      WHERE id = @Id
      ";
      _db.Execute(sql, original);
      return original;
    }
    internal void Delete(int id)
    {
      string sql = "DELETE FROM steps WHERE id = @Id LIMIT 1";
      _db.Execute(sql, new { id });
    }

  }
}