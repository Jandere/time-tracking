namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    
    string? UserRoleName { get; }

    Task<int?> GetCompanyId();
}