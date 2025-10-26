using System;
using DeadDog.Ordexp;
using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class IntegerWidgetSys : WidgetSys<int>
{
    #region 组件
    [Export] public ValueRangeComp<int> ValueRangeComp { get; private set; }
    #endregion
    #region 重写
    public override void SetValue(int value)
    {
        var v = value.CalcWithRangeStep(
            ValueRangeComp.MinValue,
            ValueRangeComp.MaxValue,
            ValueRangeComp.Step
        );
        WidgetComp.Value = v;
        WidgetComp.DisplayValue = v.ToString();
    }
    #endregion
}