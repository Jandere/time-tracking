using System.Reflection;

namespace Domain.Common;

public abstract class Enumeration : IComparable
{
    public string Name { get; set; }
    
    public int Id { get; set; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valuesMatches = Id.Equals(otherValue.Id);
        
        return typeMatches && valuesMatches;
    }

    public int CompareTo(object? other) => Id.CompareTo(((Enumeration) other!).Id);
}