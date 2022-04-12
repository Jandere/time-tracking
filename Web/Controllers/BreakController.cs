using Application.Breaks.Commands.AddBreak;
using Application.Breaks.Commands.DeleteBreak;
using Application.Breaks.Commands.FinishBreak;
using Application.Common.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers;

public class BreakController : BaseApiController
{
    [HttpPost]
    [AppAuthorize(nameof(Role.Developer))]
    public async Task<ActionResult<Result>> AddBreak([FromBody] AddBreakCommand request)
    {
        return HandleResult(await Mediator.Send(request));
    }

    [HttpPut("{id:int}")]
    [AppAuthorize(nameof(Role.Developer))]
    public async Task<ActionResult<Result>> FinishBreak(int id, [FromBody] FinishBreakCommand request)
    {
        return HandleResult(await Mediator.Send(request));
    }
    
    [HttpDelete("{id:int}")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> DeleteBreak(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteBreakCommand(id)));
    }
}