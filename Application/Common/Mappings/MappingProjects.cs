using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.UpdateProject;
using Application.Projects.Queries;
using Domain.Entities;

namespace Application.Common.Mappings;

public partial class MappingProfile
{
    private void MapProjects()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(x => x.TeamLeadFullName,
                op => op.MapFrom(
                    x => $"{x.TeamLead.Surname} {x.TeamLead.Name} {x.TeamLead.Patronymic}"))
            .ForMember(x => x.CompanyAdministratorId,
                op => op.MapFrom(
                    x => x.Company.AdministratorId));

        CreateMap<Project, ProjectDetailsDto>()
            .IncludeBase<Project, ProjectDto>()
            .ForMember(x => x.Developers,
                op => op.MapFrom(
                    x => x.Developers.Select(d => d.Developer)));

        CreateMap<CreateProjectCommand, Project>();

        CreateMap<UpdateProjectCommand, Project>();
    }
}