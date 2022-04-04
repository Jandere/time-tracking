using Domain.Common;

namespace Application.Common.Models;

public class BaseDto<T> : IAuditableEntity, IHasId<T> where T : notnull
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public T? Id { get; set; } = default;
}