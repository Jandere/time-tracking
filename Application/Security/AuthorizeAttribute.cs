using Domain.Enums;

namespace Application.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute() { }

    public AuthorizeAttribute(params Role[] roles)
    {
        Roles = roles;
    }

    public ICollection<Role> Roles { get; set; } = new List<Role>();
}