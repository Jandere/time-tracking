using Application.Common.Models;
using Application.Developers.Commands.CreateDeveloper;
using Application.Developers.Commands.DeleteDeveloper;
using Application.Developers.Commands.UpdateDeveloper;
using Application.Developers.Queries;
using Application.Developers.Queries.GetAllDevelopers;
using Application.Developers.Queries.GetDeveloperById;
using Application.Developers.Queries.GetDeveloperFullInfo;
using Application.Developers.Queries.GetDeveloperProjects;
using Application.Projects.Queries;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

public class DeveloperController : BaseApiController
{
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

    // TODO написать логику по рабочим дням
    [HttpGet("{id}/WorkDays")]
    public async Task<ActionResult> GetWorkDays(string id)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<AuthenticateRequest>> Create([FromBody] CreateDeveloperCommand request)
    {
        if (!ModelState.IsValid) return BadRequest(Result.Failure(ModelState.GetErrors()));
        
        return Ok(await Mediator.Send(request));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Result>> Update(string id, [FromBody] UpdateDeveloperCommand request)
    {
        if (id != request.Id) return BadRequest("Ids not equal");
        
        if (!ModelState.IsValid) return BadRequest(Result.Failure(ModelState.GetErrors()));
 
        return Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> Delete(string id)
    {
        return Ok(await Mediator.Send(new DeleteDeveloperCommand(id)));
    }
}