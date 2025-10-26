using Godot;

namespace RawPremiere.Components;

public partial class IntegerRangeComp : ValueRangeComp<int>
{
    #region 属性
    [Notify,Export] public override int MinValue { get => _minValue.Get(); set => _minValue.Set(value); }
    [Notify,Export] public override int MaxValue { get => _maxValue.Get(); set => _maxValue.Set(value); }
    [Notify,Export] public override int Step { get => _step.Get(); set => _step.Set(value); }
    #endregion
}