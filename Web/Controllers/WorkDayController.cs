using Application.Breaks.Queries;
using Application.Common.Models;
using Application.WorkDays.Commands.CreateWorkDay;
using Application.WorkDays.Commands.FinishWorkDay;
using Application.WorkDays.Queries;
using Application.WorkDays.Queries.GetBreaks;
using Application.WorkDays.Queries.GetWorkDayByDate;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers;

public class WorkDayController : BaseApiController
{
    [HttpPost]
    [AppAuthorize(nameof(Role.Developer))]
    public async Task<ActionResult<Result>> CreateWorkDay([FromBody] CreateWorkDayCommand request)
    {
        return HandleResult(await Mediator.Send(request));
    }

    [HttpPut("{id:int}/Finish")]
    [AppAuthorize(nameof(Role.Developer))]
    public async Task<ActionResult<Result>> FinishWorkDay(int id, [FromBody] FinishWorkDayCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids not equal");
        
        return HandleResult(await Mediator.Send(request));
    }

    [HttpGet("{date:datetime}")]
    [AppAuthorize(nameof(Role.Developer))]
    public async Task<ActionResult<WorkDayDto>> GetWorkDayByDate(DateTime date)
    {
        return Ok(await Mediator.Send(new GetWorkDayByDateQuery(date)));
    }

    [HttpGet("{id:int}/Breaks")]
    [AppAuthorize(nameof(Role.Administrator), nameof(Role.Developer))]
    public async Task<ActionResult<ICollection<BreakDto>>> GetBreaks(int id)
    {
        return Ok(await Mediator.Send(new GetBreaksQuery(id)));
    }
}