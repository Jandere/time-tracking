using Application.Common.Extensions;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Developers.Commands.UpdateDeveloper;

public class UpdateDeveloperCommand : IRequest<Result>
{
    public string Id { get; set; }
    
    public string UserName { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Name { get; set; }
    
    public string? Patronymic { get; set; }

    public decimal HourlyRate { get; set; }

    public int CompanyId { get; set; }
}

internal class UpdateDeveloperCommandHandler : IRequestHandler<UpdateDeveloperCommand, Result>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateDeveloperCommandHandler(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<Result> Handle(UpdateDeveloperCommand request, CancellationToken cancellationToken)
    {
        var developer = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == request.Id, 
                cancellationToken);

        if (developer is null)
            return Result.Failure("Administrator not found");
        
        _mapper.Map(request, developer);

        var result = await _userManager.UpdateAsync(developer);
        
        return result.ToAppResult();
    }
}