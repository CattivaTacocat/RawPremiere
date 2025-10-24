using System;
using Lombok.NET;

namespace RawPremiere.Temp.NCSTemp;

[AllArgsConstructor]
[ToString]
public partial struct AttackComp
{
    public int CurrentAtk;
    public float CriticalRatio;
    public float CriticalMultiplier;
    public int MaxAtk;
    public int MinAtk;

    public float GetActualAtk()
    {
        var rnd = new Random();
        var ratio = rnd.NextSingle();
        return ratio < CriticalRatio ? CurrentAtk * CriticalMultiplier : CurrentAtk;
    }
}