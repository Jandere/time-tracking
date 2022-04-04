namespace Application.Common.Models;

public class RegisterRequest
{
    public string UserName { get; set; } = null!;

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Patronymic { get; set; }

    public string Password { get; set; } = null!;
}