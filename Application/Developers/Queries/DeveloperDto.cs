namespace Application.Developers.Queries;

public class DeveloperDto
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Name { get; set; }
    
    public string? Patronymic { get; set; }

    public string FullName => $"{Surname} {Name} {Patronymic}";
    
    public decimal HourlyRate { get; set; }

    public string RoleName { get; set; } = null!;

    public int CompanyId { get; set; }

    public string RoleId { get; set; } = null!;

    public string CompanyName { get; set; } = string.Empty;
}