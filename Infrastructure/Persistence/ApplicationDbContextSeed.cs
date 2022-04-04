using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        var administratorRole = new AppRole(Role.Administrator.Name);
        var developerRole = new AppRole(Role.Developer.Name);
        var mainRole = new AppRole(Role.Main.Name);

        var roles = new[] {administratorRole, developerRole, mainRole};

        var existRoles = (await roleManager.Roles.ToListAsync()).Select(r => r.Name);

        var toAdd = roles.Where(r => !existRoles.Contains(r.Name));

        foreach (var role in toAdd)
        {
            await roleManager.CreateAsync(role);
        }
    }

    public static async Task SeedMain(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        var mainRole = await roleManager.FindByNameAsync(Role.Main.Name);
        
        if (mainRole == null)
            return;

        if (userManager.FindByNameAsync("main") != null)
            return;
        
        var mainUser = new AppUser
        {
            UserName = "main"
        };
        
        await userManager.CreateAsync(mainUser, "Qwerty123");
    }
    
    public static async Task SeedAdministrators(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        var administratorRole = await roleManager.FindByNameAsync(Role.Administrator.Name);

        if (administratorRole == null)
            return;

        var administrator = new AppUser
        {
            UserName = "Yan",
            Email = "yan@test.ru",
            Name = "Yan",
            Surname = "Utzhanov",
            Patronymic = "Chingisovich",
            RoleName = administratorRole.Name,
            RoleId = administratorRole.Id
        };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Qwerty123!");
        }
    }

    public static async Task SeedDevelopers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext context)
    {
        var companies = await context.Companies.ToListAsync();

        if (companies.Count == 0)
            return;

        var developerRole = await roleManager.FindByNameAsync(Role.Developer.Name);

        if (developerRole == null)
            return;

        var developers = new List<AppUser>
        {
            new AppUser
            {
                UserName = "IvanDeveloper", Email = "ivan@test.ru", Surname = "Ivanov", Name = "Ivan",
                Patronymic = "Ivanovich"
            },
            new AppUser
            {
                UserName = "ClinDeveloper", Email = "clin@test.ru", Surname = "Clinov", Name = "Clin",
                Patronymic = "Clinovich"
            },
            new AppUser
            {
                UserName = "TestDeveloper", Email = "test@test.ru", Surname = "Testov", Name = "Test",
                Patronymic = "Testovich"
            },
            new AppUser
            {
                UserName = "PetrDeveloper", Email = "petr@test.ru", Surname = "Petrov", Name = "Petr",
                Patronymic = "Petrovich"
            }
        };

        developers.ForEach(d =>
        {
            d.RoleId = developerRole.Id;
            d.RoleName = developerRole.Name;
        });
        
        var toAdd = developers
            .Where(d => userManager.Users.All(u => d.UserName != u.UserName));

        foreach (var developer in toAdd)
        {
            await userManager.CreateAsync(developer, "Qwerty123!");
        }
    }

    public static async Task SeedCompanies(ApplicationDbContext context)
    {
        var administratorId = (await context.AppUsers.FirstOrDefaultAsync(a => a.UserName == "Yan"))?.Id;

        if (administratorId == null)
            return;
        
        if (context.Companies.Any())
            return;

        var companies = new List<Company>
        {
            new() {Name = "YanCompany", AdministratorId = administratorId},
            new() {Name = "TestCompany", AdministratorId = administratorId},
        };

        context.Companies.AddRange(companies);
        await context.SaveChangesAsync();
    }

    public static async Task SeedProjects(ApplicationDbContext context)
    {
        if (context.Projects.Any())
            return;
        
        var developer = await context.AppUsers
            .Where(u => u.RoleName == Role.Developer.Name)
            .ToListAsync();
        
        if (developer.Count == 0)
            return;

        var companies = await context.Companies.ToListAsync();

        if (companies.Count == 0)
            return;

        var projects = new List<Project>
        {
            new()
            {
                Name = "TestProject", Description = "TestDescription",
                CompanyId = companies[0].Id, Developers = new List<DeveloperProject>
                {
                    new() {DeveloperId = developer[0].Id},
                    new() {DeveloperId = developer[1].Id}
                },
                TeamLeadId = developer[0].Id
            },
            new()
            {
                Name = "TestProject2", Description = "TestDescription2",
                CompanyId = companies[1].Id, Developers = new List<DeveloperProject>
                {
                    new() {DeveloperId = developer[2].Id},
                    new() {DeveloperId = developer[3].Id}
                },
                TeamLeadId = developer[2].Id
            }
        };
        
        context.Projects.AddRange(projects);
        await context.SaveChangesAsync();
    }
}