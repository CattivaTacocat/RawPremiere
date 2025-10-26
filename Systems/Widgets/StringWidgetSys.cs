using DeadDog.Ordexp;
using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class StringWidgetSys : WidgetSys<string>
{
    #region 组件
    [Export] public ValueRangeComp<int> ValueRangeComp { get; private set; }
    #endregion
    #region 重写
    public override void SetValue(string value)
    {
        var length = value.Length;
        var l = length.CalcWithRangeStep(
            ValueRangeComp.MinValue,
            ValueRangeComp.MaxValue,
            ValueRangeComp.Step
        );
        var v = value.RestrictString(l);
        WidgetComp.Value = v;
        WidgetComp.DisplayValue = v;
    }
    #endregion
}