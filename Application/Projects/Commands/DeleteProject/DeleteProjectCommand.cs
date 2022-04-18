using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommand : IRequest<Result>
{
    public DeleteProjectCommand(int id)
    {
        Id = id;
    }

    public int Id { get; }
}

internal class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .SingleOrDefaultAsync(c => request.Id == c.Id, cancellationToken);

        if (project is null)
            return Result.Failure("Project not found");

        _context.Projects.Remove(project);
        
        var result = await _context.SaveChangesAsync(cancellationToken);

        return result ? Result.Success() : Result.Failure("Error during deleting project");
    }
}