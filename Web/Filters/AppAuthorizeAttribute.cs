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
        foreach (var role in Roles)
        {
            if (!context.HttpContext.User.IsInRole(role.Name))
                context.Result = new ForbidResult();
        }
    }
}