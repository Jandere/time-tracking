using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppRole : IdentityRole
{
    public AppRole(string name) : base(name)
    {
        
    }
}