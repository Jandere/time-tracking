namespace Domain.Common;

public interface IHasId<T>
    where T : notnull
{
    public T? Id { get; set; }
}