namespace Domain.Common;

public class BaseEntity<T> : IAuditableEntity, IHasId<T>
    where T : notnull
{
    public T? Id { get; set; } = default;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}