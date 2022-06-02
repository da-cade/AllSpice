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
  public class FavoritesController : ControllerBase
  {
    private readonly FavoritesService _fs;

    public FavoritesController(FavoritesService fs)
    {
      _fs = fs;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Favorite>> Create([FromBody] Favorite favData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        favData.AccountId = userInfo.Id;
        Favorite newFav = _fs.Create(favData);
        return Ok(newFav);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<String>> Remove(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _fs.Remove(id, userInfo.Id);
        return Ok("Removed from your favorites");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}