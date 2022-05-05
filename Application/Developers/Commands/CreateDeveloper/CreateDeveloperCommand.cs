using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Developers.Commands.CreateDeveloper;

public class CreateDeveloperCommand : IRequest<AuthenticateResponse?>
{
    public string UserName { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Name { get; set; }
    
    public string? Patronymic { get; set; }
    
    public string? Position { get; set; }
    
    public string? ImgPath { get; set; }

    public string Password { get; set; } = null!;

    public decimal HourlyRate { get; set; }

    public int CompanyId { get; set; }
}

internal class CreateDeveloperCommandHandler : IRequestHandler<CreateDeveloperCommand, AuthenticateResponse?>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;

    public CreateDeveloperCommandHandler(UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager, IMapper mapper, IIdentityService identityService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _identityService = identityService;
    }
    
    public async Task<AuthenticateResponse?> Handle(CreateDeveloperCommand request, CancellationToken cancellationToken)
    {
        var userWithSameUsername = await _userManager.FindByNameAsync(request.UserName);
       
        if (userWithSameUsername is not null)
            throw new BusinessLogicException("Username already taken");
        
        var developerRole = await _roleManager.FindByNameAsync(Role.Developer.Name);
        if (developerRole is null)
            throw new NotFoundException(nameof(Role), Role.Developer.Name);
        
        var developer = _mapper.Map<AppUser>(request);
        developer.RoleName = developerRole.Name;
        developer.RoleId = developerRole.Id;

        var result = await _userManager.CreateAsync(developer, request.Password);

        if (!result.Succeeded)
            throw new BusinessLogicException("Error during creating developer");

        var response = _identityService.Login(developer);
        
        return response;
    }
}