using Domain.Enums;

namespace Application.Common.Models;

public class DeveloperRegisterRequest : RegisterRequest
{
    public decimal HourlyRate { get; set; }
    public string RoleName => Role.Developer.Name;
}