using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AppUser> AppUsers { get; }
    DbSet<AppRole> AppRoles { get; }
    
    DbSet<Company> Companies { get; }
    DbSet<Project> Projects { get; }
    DbSet<DeveloperProject> DeveloperProjects { get; }

    DbSet<WorkDay> WorkDays { get; }
    DbSet<Break> Breaks { get; }

    DbSet<WorkTask> WorkTasks { get; }
    DbSet<WorkTaskExecutor> WorkTaskExecutors { get; }

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}