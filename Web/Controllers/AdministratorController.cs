using Application.Administrators.Commands.CreateAdministrator;
using Application.Administrators.Commands.DeleteAdministrator;
using Application.Administrators.Commands.UpdateAdministrator;
using Application.Administrators.Queries;
using Application.Administrators.Queries.GetAdministratorById;
using Application.Administrators.Queries.GetAdministratorCompanies;
using Application.Administrators.Queries.GetAllAdministrators;
using Application.Common.Models;
using Application.Companies.Queries;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

public class AdministratorController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<ICollection<AdministratorDto>>> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllAdministratorsQuery()));
    }

    [HttpGet("{id}/Companies")]
    public async Task<ActionResult<ICollection<CompanyDto>>> GetCompanies(string id)
    {
        return Ok(await Mediator.Send(new GetAdministratorCompaniesQuery(id)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdministratorDto>> GetById(string id)
    {
        var administrator = await Mediator.Send(new GetAdministratorByIdQuery(id));
        return administrator is null ? NotFound() : Ok(administrator);
    }

    [HttpPost]
    public async Task<ActionResult<Result>> Create([FromBody] CreateAdministratorCommand request)
    {
        if (!ModelState.IsValid) return BadRequest(Result.Failure(ModelState.GetErrors()));
        
        return Ok(await Mediator.Send(request));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Result>> Update(string id, [FromBody] UpdateAdministratorCommand request)
    {
        if (id != request.Id) return BadRequest(Result.Failure("Ids not equal"));

        if (!ModelState.IsValid) return BadRequest(Result.Failure(ModelState.GetErrors()));

        return Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> Delete(string id)
    {
        return Ok(await Mediator.Send(new DeleteAdministratorCommand(id)));
    }
}