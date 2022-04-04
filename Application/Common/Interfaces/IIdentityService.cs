using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
    Task<(Result Result, string UserId)> CreateUserAsync(AppUser user, string password);

    Task<Result> DeleteUserAsync(string userId);
    
    Task<AuthenticateResponse> LoginAsync(AuthenticateRequest request);

    AuthenticateResponse Login(AppUser user);

    Task<AuthenticateResponse> AdministratorRegisterAsync(AdministratorRegisterRequest request);
    
    Task<AuthenticateResponse> DeveloperRegisterAsync(DeveloperRegisterRequest request);
}