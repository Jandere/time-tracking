using Domain.Common;

namespace Domain.Enums;

public class Role : Enumeration
{
    public static Role Administrator = new(1, nameof(Administrator));
    public static Role Developer = new(2, nameof(Developer));
    public static Role Main = new(3, nameof(Main));

    public Role(int id, string name) 
        : base(id, name)
    {
    }
}