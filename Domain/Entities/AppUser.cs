using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser
{
    public string? Surname { get; set; }

    public string? Name { get; set; }
    
    public string? Patronymic { get; set; }

    public decimal? HourlyRate { get; set; }

    public string? Position { get; set; }

    public string RoleName { get; set; } = null!;
    
    public string? ImgPath { get; set; }

    public int? CompanyId { get; set; }

    public string RoleId { get; set; } = null!;
    
    [ForeignKey(nameof(RoleId))]
    public AppRole? Role { get; set; }
    
    [ForeignKey(nameof(CompanyId))] 
    public Company? Company { get; set; }
    
    public IList<WorkDay> WorkDays { get; set; } = new List<WorkDay>();

    public IList<DeveloperProject> Projects { get; set; } = new List<DeveloperProject>();

    public IList<WorkTaskExecutor> WorkTasks { get; set; } = new List<WorkTaskExecutor>();

    public IList<Company> Companies { get; set; } = new List<Company>();
}