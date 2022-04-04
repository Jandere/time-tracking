using Application.Common.Models;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using Application.Projects.Commands.UpdateProject;
using Application.Projects.Commands.UpdateProjectTeamLead;
using Application.Projects.Queries;
using Application.Projects.Queries.GetAllProjects;
using Application.Projects.Queries.GetProjectById;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers;

public class ProjectController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllProjectsQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDetailsDto>> GetById(int id)
    {
        var project = await Mediator.Send(new GetProjectByIdQuery(id));
        return project is null ? NotFound() : Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<Result>> Create([FromBody] CreateProjectCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPut("{id:int}")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> ChangeTeamLead(int id, [FromBody] UpdateProjectTeamLeadCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids not equal");
        
        return Ok(await Mediator.Send(request));
    }

    [HttpPut("{id:int}")]
    [AppAuthorize(nameof(Role.Administrator), nameof(Role.Developer))]
    public async Task<ActionResult<Result>> Update(int id, [FromBody] UpdateProjectCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids not equal");

        return Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> Delete(int id)
    {
        return Ok(await Mediator.Send(new DeleteProjectCommand(id)));
    }
}