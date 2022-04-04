using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Administrators.Commands.CreateAdministrator;

public class CreateAdministratorCommand : IRequest<AuthenticateResponse>
{
    public string UserName { get; set; } = null!;
    
    public string? Surname { get; set; }

    public string? Name { get; set; }
    
    public string? Patronymic { get; set; }

    public string Password { get; set; } = null!;
}

public class CreateAdministratorCommandHandler : IRequestHandler<CreateAdministratorCommand, AuthenticateResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly RoleManager<AppRole> _roleManager;

    public CreateAdministratorCommandHandler(UserManager<AppUser> userManager, IIdentityService identityService,
        IMapper mapper, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
        _roleManager = roleManager;
    }
    
    public async Task<AuthenticateResponse> Handle(CreateAdministratorCommand request, CancellationToken cancellationToken)
    {
        var administratorRole = await _roleManager.FindByNameAsync(Role.Administrator.Name);

        if (administratorRole is null)
            throw new NotFoundException(nameof(Role), Role.Administrator.Name);
        
        var administrator = _mapper.Map<AppUser>(request);
        administrator.RoleName = administratorRole.Name;
        administrator.RoleId = administratorRole.Id;

        var result = await _userManager.CreateAsync(administrator, request.Password);

        if (!result.Succeeded)
            throw new BusinessLogicException("Error during creating administrator");

        var response = _identityService.Login(administrator);
        
        return response;
    }
}