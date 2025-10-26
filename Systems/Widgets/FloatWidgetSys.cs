using System;
using DeadDog.Ordexp;
using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class FloatWidgetSys : WidgetSys<float>
{
    #region 组件
    [Export] public StringFormatComp StringFormatComp { get; private set; }
    [Export] public ValueRangeComp<float> ValueRangeComp { get; private set; }
    #endregion
    #region 重写
    public override void SetValue(float value)
    {
        var v = value.CalcWithRangeStep(
            ValueRangeComp.MinValue,
            ValueRangeComp.MaxValue,
            ValueRangeComp.Step
        );
        WidgetComp.Value = v;
        WidgetComp.DisplayValue = v.ToString(StringFormatComp.Format);
    }
    #endregion
}