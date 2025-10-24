using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DeadDog.Ordexp;

public static class EnumerableExpends
{
    #region 决策
    public static bool IsEnumerableNullOrEmpty<T>(this IEnumerable<T> origin) => origin is null || !origin.Any();

    public static bool CantAssignValue<T>(this IEnumerable<T> origin, IEnumerable<T> value,bool canEmpty = false)
    {
        if (!canEmpty && value is null) return true;
        return origin.Equals(value) || origin.SequenceEqual(value);
    }
    #endregion
}