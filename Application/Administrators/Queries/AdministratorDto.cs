namespace Application.Administrators.Queries;

public class AdministratorDto
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;
    
    public string? Surname { get; set; }

    public string? Name { get; set; }
    
    public string? Patronymic { get; set; }

    public string FullName => $"{Surname} {Name} {Patronymic}";

    public string RoleName { get; set; } = null!;

    public string RoleId { get; set; } = null!;
}