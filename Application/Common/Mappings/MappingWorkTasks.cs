using Application.WorkTasks.Commands.CreateWorkTask;
using Application.WorkTasks.Commands.UpdateWorkTask;
using Application.WorkTasks.Queries;
using Domain.Entities;

namespace Application.Common.Mappings;

public partial class MappingProfile
{
    private void MapWorkTask()
    {
        CreateMap<WorkTask, WorkTaskDto>();
        CreateMap<CreateWorkTaskCommand, WorkTask>();
        CreateMap<UpdateWorkTaskCommand, WorkTask>();
    }
}