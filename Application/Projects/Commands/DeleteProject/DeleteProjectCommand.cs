using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public DeleteProjectCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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