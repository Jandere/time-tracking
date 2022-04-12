using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? TeamLeadId { get; set; }
}

internal class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProjectCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == request.Id
                                      && p.Company.AdministratorId == _currentUserService.UserId,
                cancellationToken);
        
        _mapper.Map(request, project);
        
        return await _context.SaveChangesAsync(cancellationToken) 
            ? Result.Success() : Result.Failure("Error during updating company");
    }
}