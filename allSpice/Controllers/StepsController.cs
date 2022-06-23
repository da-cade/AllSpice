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
  public class StepsController : ControllerBase
  {
    private readonly StepsService _ss;

    public StepsController(StepsService ss)
    {
      _ss = ss;
    }

    [HttpGet("{id}")]
    public ActionResult<Step> GetStep(int id)
    {
      try
      {
        Step step = _ss.GetStep(id);
        return Ok(step);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Step>> Create([FromBody] Step stepData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        stepData.CreatorId = userInfo.Id;
        Step newStep = _ss.Create(stepData);
        return Ok(newStep);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut]
    public async Task<ActionResult<List<Step>>> EditMany([FromBody] List<Step> edits)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        List<Step> updates = _ss.EditMany(edits, userInfo.Id);
        return Ok(updates);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    // [HttpPut("{id}")]
    // [Authorize]
    // public async Task<ActionResult<Step>> Edit([FromBody] Step stepData)
    // {
    //   try
    //   {
    //     Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
    //     Step update = _ss.Edit(stepData, userInfo.Id);
    //     return Ok(update);
    //   }
    //   catch (Exception e)
    //   {
    //     return BadRequest(e.Message);
    //   }
    // }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<String>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _ss.Delete(id, userInfo.Id);
        return Ok("Deleted successfully");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}