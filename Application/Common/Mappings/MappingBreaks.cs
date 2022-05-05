using Application.Breaks.Commands.AddBreak;
using Application.Breaks.Queries;
using Domain.Entities;

namespace Application.Common.Mappings;

public partial class MappingProfile
{
    private void MapBreaks()
    {
        CreateMap<Break, BreakDto>();
        CreateMap<AddBreakCommand, Break>()
            .ForMember(x => x.StartDate,
                op => op.MapFrom(
                    x => x.StartTime));
    }
}