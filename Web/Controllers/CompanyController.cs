using Application.Common.Models;
using Application.Companies.Commands.CreateCompany;
using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Queries;
using Application.Companies.Queries.GetAllCompanies;
using Application.Companies.Queries.GetById;
using Application.Companies.Queries.GetCompanyDevelopers;
using Application.Companies.Queries.GetCompanyProjects;
using Application.Developers.Queries;
using Application.Projects.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers;

public class CompanyController : BaseApiController
{
    [HttpGet]
    [AppAuthorize(nameof(Role.Main))]
    public async Task<ActionResult<ICollection<CompanyDto>>> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllCompaniesQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CompanyDetailsDto>> GetById(int id)
    {
        var company = await Mediator.Send(new GetCompanyByIdQuery(id));
        return company == null ? NotFound() : Ok(company);
    }

    [HttpGet("{id:int}/Developers")]
    public async Task<ActionResult<ICollection<DeveloperDto>>> GetDevelopers(int id)
    {
        return Ok(await Mediator.Send(new GetCompanyDevelopersQuery(id)));
    }

    [HttpGet("{id:int}/Projects")]
    public async Task<ActionResult<ICollection<ProjectDto>>> GetProjects(int id)
    {
        return Ok(await Mediator.Send(new GetCompanyProjectsQuery(id)));
    }

    [HttpPost]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> Create([FromBody] CreateCompanyCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPut("{id:int}")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> Update(int id, [FromBody] UpdateCompanyCommand request)
    {
        if (id != request.Id)
            return BadRequest("Ids not equal");
        
        return Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    [AppAuthorize(nameof(Role.Administrator))]
    public async Task<ActionResult<Result>> Delete(int id)
    {
        return Ok(await Mediator.Send(new DeleteCompanyCommand(id)));
    }
}