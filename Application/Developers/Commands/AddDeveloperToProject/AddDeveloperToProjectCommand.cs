using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Commands.AddDeveloperToProject;

public class AddDeveloperToProjectCommand : IRequest<Result>
{
    public string DeveloperId { get; set; }

    public int ProjectId { get; set; }

    public AddDeveloperToProjectCommand(string developerId, int projectId)
    {
        DeveloperId = developerId;
        ProjectId = projectId;
    }
}

internal class AddDeveloperToProjectCommandHandler : IRequestHandler<AddDeveloperToProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddDeveloperToProjectCommandHandler(IApplicationDbContext context, 
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(AddDeveloperToProjectCommand request, CancellationToken cancellationToken)
    {
        var userExist = await _context.AppUsers
            .AnyAsync(x => x.Id == request.DeveloperId 
                                       && x.RoleName == Role.Developer.Name, 
                cancellationToken);

        if (!userExist)
            throw new NotFoundException("User not found");

        var project = await _context.Projects
            .Include(x => x.Company)
            .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

        if (project is null)
            throw new NotFoundException("Company not found");

        if (project.Company?.AdministratorId != _currentUserService.UserId)
            throw new BusinessLogicException("Current administrator not project's administrator");

        var relation = new DeveloperProject
        {
            DeveloperId = request.DeveloperId,
            ProjectId = request.ProjectId
        };

        _context.DeveloperProjects.Add(relation);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during updating relation");
    }
}