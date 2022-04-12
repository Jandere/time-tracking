using Application.Administrators.Commands.CreateAdministrator;
using Application.Administrators.Commands.DeleteAdministrator;
using Application.Administrators.Commands.UpdateAdministrator;
using Application.Administrators.Queries;
using Application.Administrators.Queries.GetAdministratorById;
using Application.Administrators.Queries.GetAdministratorCompanies;
using Application.Administrators.Queries.GetAllAdministrators;
using Application.Common.Models;
using Application.Companies.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.Filters;

namespace Web.Controllers;

public class AdministratorController : BaseApiController
{
    [HttpGet]
    [AppAuthorize(nameof(Role.Main))]
    public async Task<ActionResult<ICollection<AdministratorDto>>> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllAdministratorsQuery()));
    }

    [HttpGet("{id}/Companies")]
    [AppAuthorize(nameof(Role.Main), nameof(Role.Administrator))]
    public async Task<ActionResult<ICollection<CompanyDto>>> GetCompanies(string id)
    {
        return Ok(await Mediator.Send(new GetAdministratorCompaniesQuery(id)));
    }

    [HttpGet("{id}")]
    [AppAuthorize]
    public async Task<ActionResult<AdministratorDto>> GetById(string id)
    {
        var administrator = await Mediator.Send(new GetAdministratorByIdQuery(id));
        return administrator is null ? NotFound() : Ok(administrator);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticateResponse>> Create([FromBody] CreateAdministratorCommand request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.GetErrors());
        
        return Ok(await Mediator.Send(request));
    }

    [HttpPut("{id}")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> Update(string id, [FromBody] UpdateAdministratorCommand request)
    {
        if (id != request.Id) return BadRequest(Result.Failure("Ids not equal"));

        if (!ModelState.IsValid) return BadRequest(Result.Failure(ModelState.GetErrors()));

        return HandleResult(await Mediator.Send(request));
    }

    [HttpDelete("{id}")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteAdministratorCommand(id)));
    }
}