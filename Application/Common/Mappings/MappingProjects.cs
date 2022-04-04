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
                    x => $"{x.TeamLead.Surname} {x.TeamLead.Name} {x.TeamLead.Patronymic}"));

        CreateMap<Project, ProjectDetailsDto>()
            .ForMember(x => x.TeamLead,
                op => op.MapFrom(
                    x => x.TeamLead))
            .ForMember(x => x.Developers,
                op => op.MapFrom(
                    x => x.Developers.Select(d => d.Developer)))
            .ForMember(x => x.TeamLeadFullName,
                op => op.MapFrom(
                    x => $"{x.TeamLead.Surname} {x.TeamLead.Name} {x.TeamLead.Patronymic}"));

        CreateMap<CreateProjectCommand, Project>();

        CreateMap<UpdateProjectCommand, Project>();
    }
}