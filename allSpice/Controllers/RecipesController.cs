using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using allSpice.Models;
using allSpice.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace allSpice.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class RecipesController : ControllerBase
  {
    private readonly RecipesService _rs;
    private readonly IngredientsService _igs;
    private readonly StepsService _ss;
    private readonly FavoritesService _fs;

    public RecipesController(RecipesService rs, IngredientsService igs, StepsService ss, FavoritesService fs)
    {
      _rs = rs;
      _igs = igs;
      _ss = ss;
      _fs = fs;
    }

    [HttpGet]
    public ActionResult<List<Recipe>> GetAllRecipes()
    {
      try
      {
        List<Recipe> recipes = _rs.GetAllRecipes();
        return Ok(recipes);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("categories/{category}")]
    public ActionResult<List<Recipe>> GetRecipesByCategory(string category)
    {
      try
      {
        List<Recipe> recipes = _rs.GetRecipesByCategory(category);
        return Ok(recipes);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}/ingredients")]
    public ActionResult<List<Ingredient>> GetIngredientsByRecipe(int id)
    {
      try
      {
        List<Ingredient> ingredients = _igs.GetIngredientsByRecipe(id);
        return Ok(ingredients);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}/favorites")]
    public ActionResult<List<Favorite>> GetFavoritesByRecipe(int id)
    {
      try
      {
        List<Favorite> favorites = _fs.GetFavoritesByRecipe(id);
        return Ok(favorites);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}/steps")]
    public ActionResult<List<Step>> GetStepsByRecipe(int id)
    {
      try
      {
        List<Step> steps = _ss.GetStepsByRecipe(id);
        return Ok(steps);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Recipe> GetRecipe(int id)
    {
      try
      {
        Recipe recipe = _rs.GetRecipe(id);
        return Ok(recipe);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Recipe>> Create([FromBody] Recipe recData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        recData.CreatorId = userInfo.Id;
        Recipe newRecipe = _rs.Create(recData);
        return Ok(newRecipe);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Recipe>> Edit(int id, [FromBody] Recipe recData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        recData.CreatorId = userInfo.Id;
        recData.Id = id;
        Recipe update = _rs.Edit(id, recData);
        return Ok(update);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<string>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _rs.Delete(id, userInfo.Id);
        return Ok("Deletion Successful");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}