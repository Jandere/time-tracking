using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IDateTime _dateTime;
    private readonly AuthOptions _authOptions;
    private readonly RoleManager<AppRole> _roleManager;

    public IdentityService(UserManager<AppUser> userManager,
        IOptions<AuthOptions> authOptions,
        IDateTime dateTime, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _authOptions = authOptions.Value;
        _dateTime = dateTime;
        _roleManager = roleManager;
    }
    
    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new AppUser
        {
            UserName = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(AppUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
        
        if (user == null) 
            return Result.Failure("User not found");

        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
    
    public async Task<AuthenticateResponse> LoginAsync(AuthenticateRequest request)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.UserName == request.Username);

        if (user == null)
            throw new NotFoundException("User", request.Username);
        
        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
            throw new WrongPasswordException();

        var token = GenerateJwtToken(user);

        return new AuthenticateResponse(user.UserName, user.Id, token, user.RoleName);
    }

    public AuthenticateResponse Login(AppUser user)
    {
        var token = GenerateJwtToken(user);

        return new AuthenticateResponse(user.UserName, user.Id, token, user.RoleName);
    }

    public async Task<AuthenticateResponse> AdministratorRegisterAsync(AdministratorRegisterRequest request)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == request.UserName))
            throw new UserAlreadyExistException();

        var role = await _roleManager.FindByNameAsync(request.RoleName);

        var user = new AppUser
        {
            UserName = request.UserName,
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic,
            RoleId = role.Id,
            RoleName = role.Name
        };

        var result = await CreateUserAsync(user, request.Password);

        if (result.Result.Succeeded)
            return Login(user);

        throw new Exception("Error while creating administrator");
    }

    public async Task<AuthenticateResponse> DeveloperRegisterAsync(DeveloperRegisterRequest request)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == request.UserName))
            throw new UserAlreadyExistException();

        var role = await _roleManager.FindByNameAsync(request.RoleName);
        
        var user = new AppUser
        {
            UserName = request.UserName,
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic,
            RoleId = role.Id,
            RoleName = role.Name,
            HourlyRate = request.HourlyRate
        };

        var result = await CreateUserAsync(user, request.Password);

        if (result.Result.Succeeded)
            return Login(user);

        throw new Exception("Error while creating developer");
    }

    private string GenerateJwtToken(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authOptions.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.RoleName)
            }),
            Expires = _dateTime.Now.AddDays(_authOptions.Lifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _authOptions.Audience,
            Issuer = _authOptions.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}