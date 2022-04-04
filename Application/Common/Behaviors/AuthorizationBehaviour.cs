using System.Reflection;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Security;
using MediatR;

namespace Application.Common.Behaviors;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public AuthorizationBehaviour(
        ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        _currentUserService = currentUserService;
        _identityService = identityService;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToArray();

        if (!authorizeAttributes.Any()) return await next();
        
        if (_currentUserService.UserId == null)
        {
            throw new UnauthorizedAccessException();
        }

        var authorizeAttributesWithRoles = authorizeAttributes
            .Where(a => a.Roles.Any()).ToArray();

        if (!authorizeAttributesWithRoles.Any()) return await next();
            
        var authorized = false;

        foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles))
        {
            foreach (var role in roles)
            {
                var isInRole = await _identityService.IsInRoleAsync(_currentUserService.UserId, role.Name);
                if (!isInRole) continue;

                authorized = true;
                break;
            }
        }

        if (!authorized)
        {
            throw new ForbiddenAccessException();
        }

        return await next();
    }
}