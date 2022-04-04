namespace Domain.Common;

public struct Percentage
{
    private readonly decimal _value;

    public Percentage(decimal value)
    {
        _value = value;
    }

    public static implicit operator Percentage(int value)
    {
        return value.Percent();
    }

    public static decimal operator *(decimal a, Percentage b)
    {
        return a * b._value;
    }
    
    public static decimal operator *(Percentage a, decimal b)
    {
        return a._value * b;
    }

    public static Percentage operator +(Percentage a, Percentage b)
    {
        return new Percentage(a._value + b._value);
    }

    public static bool operator ==(Percentage a, Percentage b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Percentage a, Percentage b)
    {
        return !a.Equals(b);
    }

    public bool Equals(Percentage other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Percentage other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}

public static class PercentageExtensions
{
    public static Percentage Percent(this int value)
    {
        return new Percentage(value / 100.0m);
    }
    
    public static Percentage Percent(this decimal value)
    {
        return new Percentage(value / 100.0m);
    }
}