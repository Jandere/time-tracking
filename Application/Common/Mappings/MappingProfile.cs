using AutoMapper;

namespace Application.Common.Mappings;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        MapAdministrators();
        MapDevelopers();
        MapCompanies();
        MapProjects();
        MapWorkDays();
        MapBreaks();
        MapWorkTask();
    }
}