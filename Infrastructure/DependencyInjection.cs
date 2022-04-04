using System.Text;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")!,
                act => 
                    act.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        var builder = services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequiredLength = 4;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireNonAlphanumeric = false;
        });
        
        var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
        
        identityBuilder.AddRoles<AppRole>();
        identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
        identityBuilder.AddRoleManager<RoleManager<AppRole>>();
        identityBuilder.AddSignInManager<SignInManager<AppUser>>();

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["AuthOptions:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["AuthOptions:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["AuthOptions:Key"]!))
                };
            });
        
        services.AddAuthorization();

        return services;
    }
}