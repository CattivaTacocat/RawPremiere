using System;
using Godot;

namespace RawPremiere.Components;

public partial class ValueRangeComp<T> : Node where T : IComparable<T>
{
    #region 属性
    [Notify] public virtual T MinValue { get => _minValue.Get(); set=> _minValue.Set(value); }
    [Notify] public virtual T MaxValue { get => _maxValue.Get(); set => _maxValue.Set(value); }
    [Notify] public virtual T Step { get => _step.Get(); set=> _step.Set(value); }
    #endregion
}