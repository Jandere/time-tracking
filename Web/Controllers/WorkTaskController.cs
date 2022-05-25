using Application.Common.Models;
using Application.WorkTasks.Commands.AddDeveloperToWorkTask;
using Application.WorkTasks.Commands.CreateWorkTask;
using Application.WorkTasks.Commands.DeleteWorkTask;
using Application.WorkTasks.Commands.FinishWorkTask;
using Application.WorkTasks.Commands.RemoveDeveloperToWorkTask;
using Application.WorkTasks.Commands.UpdateWorkTask;
using Application.WorkTasks.Queries;
using Application.WorkTasks.Queries.GetWorkTask;
using Application.WorkTasks.Queries.GetWorkTasks;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers;

public class WorkTaskController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Result>> CreateWorkTask([FromBody] CreateWorkTaskCommand request)
    {
        return HandleResult(await Mediator.Send(request));
    }

    [HttpPut("Finish/{id:int}")]
    public async Task<ActionResult<Result>> FinishWorkTask(int id, [FromBody] FinishWorkTaskCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids are not equal");
        
        return HandleResult(await Mediator.Send(request));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Result>> UpdateWorkTask(int id, [FromBody] UpdateWorkTaskCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids are not equal");
        
        return HandleResult(await Mediator.Send(request));
    }

    [HttpPost("{id:int}/AddDeveloper")]
    public async Task<ActionResult<Result>> AddDeveloperToWorkTask(int id, [FromBody] AddDeveloperToWorkTaskCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids are not equal");
        
        return HandleResult(await Mediator.Send(request));
    }

    [HttpDelete("{id:int}/RemoveDeveloper")]
    public async Task<ActionResult<Result>> RemoveDeveloperFromWorkTask(int id, [FromBody] RemoveDeveloperFromWorkTaskCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids are not equal");
        
        return HandleResult(await Mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Result>> DeleteWorkTask(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteWorkTaskCommand(id)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkTaskDto>> GetWorkTask(int id)
    {
        var workTask = await Mediator.Send(new GetWorkTaskQuery(id));
        return workTask is null ? NotFound() : Ok(workTask);
    }

    [HttpGet]
    [AppAuthorize]
    public async Task<ActionResult<ICollection<WorkTaskDto>>> GetWorkTasks()
    {
        return Ok(await Mediator.Send(new GetWorkTasksQuery()));
    }
}