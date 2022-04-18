using Domain.Entities;
using Hangfire;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class ApplicationExtensions
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (context.Database.IsNpgsql())
            {
                await context.Database.MigrateAsync();
            }

            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

            await ApplicationDbContextSeed.SeedRoles(roleManager);
            await ApplicationDbContextSeed.SeedAdministrators(userManager, roleManager);
            await ApplicationDbContextSeed.SeedCompanies(context);
            await ApplicationDbContextSeed.SeedDevelopers(userManager, roleManager, context);
            await ApplicationDbContextSeed.SeedProjects(context);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            logger.LogError(ex, "An error occurred while migrating or seeding the database.");

            throw;
        }
    }

    public static IApplicationBuilder UseHangFire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("/jobs");
        HangFireJobScheduler.SetJobs();
        return app;
    }
}