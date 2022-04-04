using Application.Common.Extensions;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Developers.Commands.DeleteDeveloper;

public class DeleteDeveloperCommand : IRequest<Result>
{
    public string Id { get; }

    public DeleteDeveloperCommand(string id)
    {
        Id = id;
    }
}

internal class DeleteDeveloperCommandHandler : IRequestHandler<DeleteDeveloperCommand, Result>
{
    private readonly UserManager<AppUser> _userManager;

    public DeleteDeveloperCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result> Handle(DeleteDeveloperCommand request, CancellationToken cancellationToken)
    {
        var developer = await _userManager.FindByIdAsync(request.Id);

        if (developer is null)
            return Result.Failure("Developer not found");

        var result = await _userManager.DeleteAsync(developer);

        return result.ToAppResult();
    }
}