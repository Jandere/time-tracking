using Application.Developers.Commands.CreateDeveloper;
using Application.Developers.Commands.UpdateDeveloper;
using Application.Developers.Queries;
using Domain.Entities;

namespace Application.Common.Mappings;

public partial class MappingProfile
{
    private void MapDevelopers()
    {
        CreateMap<CreateDeveloperCommand, AppUser>();
        CreateMap<UpdateDeveloperCommand, AppUser>();

        CreateMap<AppUser, DeveloperDto>()
            .ForMember(x => x.CompanyName,
                op => op.MapFrom(
                    x => x.Company.Name));

        CreateMap<AppUser, DeveloperFullDto>()
            .ForMember(x => x.CompanyName,
                op => op.MapFrom(
                    x => x.Company.Name))
            .ForMember(x => x.Projects,
                op => op.MapFrom(
                    x => x.Projects.Select(p => p.Project)));
    }
}