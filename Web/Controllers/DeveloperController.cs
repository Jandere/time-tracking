using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Developers.Commands.AddDeveloperToProject;
using Application.Developers.Commands.CreateDeveloper;
using Application.Developers.Commands.DeleteDeveloper;
using Application.Developers.Commands.UpdateDeveloper;
using Application.Developers.Queries;
using Application.Developers.Queries.GetAllDevelopers;
using Application.Developers.Queries.GetDeveloperById;
using Application.Developers.Queries.GetDeveloperFullInfo;
using Application.Developers.Queries.GetDeveloperProjects;
using Application.Developers.Queries.GetDeveloperWorkDays;
using Application.Developers.Queries.GetDeveloperWorkTasks;
using Application.Projects.Queries;
using Application.WorkDays.Queries;
using Application.WorkTasks.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers;

public class DeveloperController : BaseApiController
{
    private readonly ICurrentUserService _currentUserService;

    public DeveloperController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<DeveloperDto>>> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllDevelopersQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeveloperDto>> GetById(string id)
    {
        var developer = await Mediator.Send(new GetDeveloperByIdQuery(id));
        
        return developer == null ? NotFound() : Ok(developer);
    }

    [HttpGet("{id}/Full")]
    public async Task<ActionResult<DeveloperFullDto>> GetFullInfo(string id)
    {
        var developer = await Mediator.Send(new GetDeveloperFullInfoQuery(id));
        
        return developer == null ? NotFound() : Ok(developer);
    }

    [HttpGet("{id}/Projects")]
    public async Task<ActionResult<ICollection<ProjectDto>>> GetProjects(string id)
    {
        return Ok(await Mediator.Send(new GetDeveloperProjectsQuery(id)));
    }

    [HttpGet("WorkDays")]
    [AppAuthorize(nameof(Role.Developer))]
    public async Task<ActionResult<ICollection<WorkDayDto>>> GetWorkDaysForDeveloper(DateTime dateFrom, DateTime dateTo)
    {
        return Ok(await Mediator.Send(new GetDeveloperWorkDaysQuery(_currentUserService.UserId!, dateFrom, dateTo)));
    }

    [HttpGet("{id}/WorkDays")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<ICollection<WorkDayDto>>> GetWorkDays(string id, DateTime dateFrom, DateTime dateTo)
    {
        return Ok(await Mediator.Send(new GetDeveloperWorkDaysQuery(id, dateFrom, dateTo)));
    }

    [HttpGet("{id}/WorkTasks")]
    public async Task<ActionResult<ICollection<WorkTaskDto>>> GetWorkTasks(string id)
    {
        return Ok(await Mediator.Send(new GetDeveloperWorkTasksQuery(id)));
    }

    [HttpPost]
    public async Task<ActionResult<AuthenticateRequest>> Create([FromBody] CreateDeveloperCommand request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.GetErrors());
        
        return Ok(await Mediator.Send(request));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Result>> Update(string id, [FromBody] UpdateDeveloperCommand request)
    {
        if (id != request.Id) return BadRequest("Ids not equal");
        
        if (!ModelState.IsValid) return BadRequest(Result.Failure(ModelState.GetErrors()));
 
        return HandleResult(await Mediator.Send(request));
    }

    [HttpPut("{id}/Project")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> AddProject(string id, [FromBody] AddDeveloperToProjectCommand request)
    {
        return HandleResult(await Mediator.Send(request));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteDeveloperCommand(id)));
    }
}