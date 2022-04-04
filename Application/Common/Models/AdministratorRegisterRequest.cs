using Domain.Enums;

namespace Application.Common.Models;

public class AdministratorRegisterRequest : RegisterRequest
{
    public string RoleName => Role.Administrator.Name;
}