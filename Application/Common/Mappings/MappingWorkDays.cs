using Application.WorkDays.Commands;
using Application.WorkDays.Commands.CreateWorkDay;
using Application.WorkDays.Queries;
using Domain.Entities;

namespace Application.Common.Mappings;

public partial class MappingProfile
{
    private void MapWorkDays()
    {
        CreateMap<WorkDay, WorkDayDto>();
        CreateMap<CreateWorkDayCommand, WorkDay>();
    }
}