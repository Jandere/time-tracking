using Application.Common.Extensions;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Administrators.Commands.DeleteAdministrator;

public class DeleteAdministratorCommand : IRequest<Result>
{
    public DeleteAdministratorCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

internal class DeleteAdministratorCommandHandler : IRequestHandler<DeleteAdministratorCommand, Result>
{
    private readonly UserManager<AppUser> _userManager;

    public DeleteAdministratorCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result> Handle(DeleteAdministratorCommand request, CancellationToken cancellationToken)
    {
        var administrator = await _userManager.FindByIdAsync(request.Id);

        if (administrator is null)
            return Result.Failure("Administrator not found");
        
        var result = await _userManager.DeleteAsync(administrator);

        return result.ToAppResult();
    }
}