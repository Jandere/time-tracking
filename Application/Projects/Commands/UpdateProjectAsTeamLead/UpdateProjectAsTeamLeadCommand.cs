using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProjectAsTeamLead;

public class UpdateProjectAsTeamLeadCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}

internal class UpdateProjectAsTeamLeadCommandHandler : IRequestHandler<UpdateProjectAsTeamLeadCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProjectAsTeamLeadCommandHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(UpdateProjectAsTeamLeadCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id
                                      && p.TeamLeadId == _currentUserService.UserId,
                cancellationToken);
        
        if (project is null)
            return Result.Failure("Project not found");

        _mapper.Map(request, project);
        
        return await _context.SaveChangesAsync(cancellationToken) 
            ? Result.Success() : Result.Failure("Error during updating company");
    }
}