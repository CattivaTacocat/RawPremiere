using System;
using Godot;

namespace DeadDog.Ordexp;

public static class ObjectExpends
{
    #region 决策
    /// <summary>
    /// 能否赋值
    /// </summary>
    /// <param name="origin">原值</param>
    /// <param name="value">新值</param>
    /// <param name="canEmpty">新值能否为所谓的空情况</param>
    /// <returns>不能赋值则为true，反之为false</returns>
    public static bool CantAssignValue(this object origin, object value,bool canEmpty = false)
    {
        if (!canEmpty) return value is null || origin.Equals(value);
        if (origin is null) return value is null;
        return origin.Equals(value);
    }
    #endregion
}