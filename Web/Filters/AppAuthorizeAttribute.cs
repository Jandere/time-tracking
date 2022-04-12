using System.Security.Claims;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Filters;

public class AppAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public AppAuthorizeAttribute()
    { }

    public AppAuthorizeAttribute(params Role[] roles)
    {
        Roles = roles;
    }
    
    public AppAuthorizeAttribute(params string[] roleNames)
    {
        var roles = Enumeration.GetAll<Role>().ToList();

        Roles = roles.Where(r => roleNames.Contains(r.Name)).ToArray();
    } 

    public ICollection<Role> Roles = new List<Role>();

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var role = Role.GetRoleByName(context.HttpContext.User.FindFirstValue(ClaimTypes.Role));

        if (Roles.Count > 0 && (role == null || !Roles.Contains(role)))
            context.Result = new ForbidResult();
    }
}