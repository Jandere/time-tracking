using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
{
    private readonly IDateTime _dateTime;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IDateTime dateTime) : base(options)
    {
        _dateTime = dateTime;
    }

    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AppRole> AppRoles => Set<AppRole>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<DeveloperProject> DeveloperProjects => Set<DeveloperProject>();

    public DbSet<WorkDay> WorkDays => Set<WorkDay>();
    public DbSet<Break> Breaks => Set<Break>();

    public new async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            entry.Entity.UpdatedAt = entry.State switch
            {
                EntityState.Modified => _dateTime.Now,
                _ => entry.Entity.UpdatedAt
            };
        }

        return await base.SaveChangesAsync(cancellationToken) > 0;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AppUser>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Developers);

        builder.Entity<AppUser>()
            .HasMany(u => u.Companies)
            .WithOne(c => c.Administrator);
        
        base.OnModelCreating(builder);
    }
}