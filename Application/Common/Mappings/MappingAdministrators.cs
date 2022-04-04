using Application.Administrators.Commands.CreateAdministrator;
using Application.Administrators.Commands.UpdateAdministrator;
using Application.Administrators.Queries;
using Domain.Entities;

namespace Application.Common.Mappings;

public partial class MappingProfile
{
    private void MapAdministrators()
    {
        CreateMap<CreateAdministratorCommand, AppUser>();
        CreateMap<AppUser, AdministratorDto>();
        CreateMap<UpdateAdministratorCommand, AppUser>();
    }
}