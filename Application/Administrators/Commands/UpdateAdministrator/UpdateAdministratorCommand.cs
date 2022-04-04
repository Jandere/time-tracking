using Application.Common.Extensions;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Administrators.Commands.UpdateAdministrator;

public class UpdateAdministratorCommand : IRequest<Result>
{
    public string Id { get; set; } = null!;
    
    public string UserName { get; set; } = null!;
    
    public string? Surname { get; set; }

    public string? Name { get; set; }
    
    public string? Patronymic { get; set; }
}

internal class UpdateAdministratorCommandHandler : IRequestHandler<UpdateAdministratorCommand, Result>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateAdministratorCommandHandler(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<Result> Handle(UpdateAdministratorCommand request, CancellationToken cancellationToken)
    {
        var administrator = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == request.Id, 
                cancellationToken);

        if (administrator is null)
            return Result.Failure("Administrator not found");
        
        _mapper.Map(request, administrator);

        var result = await _userManager.UpdateAsync(administrator);
        
        return result.ToAppResult();
    }
}