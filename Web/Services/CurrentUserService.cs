using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserRoleName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
    public async Task<int?> GetCompanyId()
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(user => user.Id == UserId);

        return user?.CompanyId;
    }
}
