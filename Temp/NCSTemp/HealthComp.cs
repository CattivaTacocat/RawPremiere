using System;
using Lombok.NET;

namespace RawPremiere.Temp.NCSTemp;

[AllArgsConstructor]
[ToString]
public partial struct HealthComp : IEquatable<HealthComp>
{
    [Property] private int _currentHealth;
    [Property] private int _maxHealth;

    public bool Equals(HealthComp other)
    {
        return _currentHealth == other._currentHealth && _maxHealth == other._maxHealth;
    }

    public override bool Equals(object obj)
    {
        return obj is HealthComp other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_currentHealth, _maxHealth);
    }
}