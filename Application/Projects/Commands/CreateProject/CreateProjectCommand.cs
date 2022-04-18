using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<Result>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? TeamLeadId { get; set; }

    public int CompanyId { get; set; }
}

internal class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (request.TeamLeadId != null)
        {
            var teamLead = await _context.AppUsers
                .SingleOrDefaultAsync(u => u.Id == request.TeamLeadId 
                                           && u.UserName == Role.Developer.Name, cancellationToken);
            
            if (teamLead is null)
                return Result.Failure("Team lead not found");
        }

        var isCompanyExist =
            await _context.Companies
                .AnyAsync(x => x.Id == request.CompanyId, cancellationToken);
        
        if (!isCompanyExist) 
            return Result.Failure("Company not found");
        
        var project = _mapper.Map<Project>(request);
        
        _context.Projects.Add(project);
        
        return await _context.SaveChangesAsync(cancellationToken) 
            ? Result.Success() : Result.Failure("Error during creating project");
    }
}