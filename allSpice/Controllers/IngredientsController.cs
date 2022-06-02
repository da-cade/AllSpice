using System;
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

  public class IngredientsController : ControllerBase
  {
    private readonly IngredientsService _igs;

    public IngredientsController(IngredientsService igs)
    {
      _igs = igs;
    }

    [HttpGet("{id}")]
    public ActionResult<Ingredient> GetIngredient(int id)
    {
      try
      {
        Ingredient ingredient = _igs.GetIngredient(id);
        return Ok(ingredient);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Ingredient>> Create([FromBody] Ingredient ingyData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        ingyData.CreatorId = userInfo.Id;
        Ingredient newIng = _igs.Create(ingyData);
        return Ok(newIng);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Ingredient>> Edit([FromBody] Ingredient ingyData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Ingredient update = _igs.Edit(ingyData, userInfo.Id);
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
        _igs.Delete(id, userInfo.Id);
        return Ok("Deletion Successful");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}