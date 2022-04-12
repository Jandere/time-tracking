using Application.Breaks.Queries;
using Domain.Entities;

namespace Application.Common.Mappings;

public partial class MappingProfile
{
    private void MapBreaks()
    {
        CreateMap<Break, BreakDto>();
    }
}