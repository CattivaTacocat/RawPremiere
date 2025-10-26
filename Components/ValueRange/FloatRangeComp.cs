using Godot;

namespace RawPremiere.Components;

public partial class FloatRangeComp : ValueRangeComp<float>
{
    #region 属性
    [Notify,Export] public float MinValue { get => _minValue.Get(); set=> _minValue.Set(value); }
    [Notify,Export] public float MaxValue { get => _maxValue.Get(); set => _maxValue.Set(value); }
    [Notify,Export] public float Step { get => _step.Get(); set=> _step.Set(value); }
    #endregion
}