using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProjectTeamLead;

public class UpdateProjectTeamLeadCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public string TeamLeadId { get; set; } = null!;
}

internal class UpdateProjectTeamLeadCommandHandler : IRequestHandler<UpdateProjectTeamLeadCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProjectTeamLeadCommandHandler(IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<Result> Handle(UpdateProjectTeamLeadCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == request.Id 
                                      && p.Company.AdministratorId == _currentUserService.UserId,
                cancellationToken);
        
        if (project is null)
            return Result.Failure("Project is not found");

        project.TeamLeadId = request.TeamLeadId;

        return await _context.SaveChangesAsync(cancellationToken) ? 
            Result.Success() : Result.Failure("Error during update project teamLead");
    }
}