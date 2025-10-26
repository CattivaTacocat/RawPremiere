using System;
using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class ColorWidgetSys : WidgetSys<Color>
{
    #region 组件
    [Export] public ColorWidgetComp WidgetComp { get; private set; }
    [Export] public ValueRangeComp<float> RRangeComp { get; private set; }
    [Export] public ValueRangeComp<float> GRangeComp { get; private set; }
    [Export] public ValueRangeComp<float> BRangeComp { get; private set; }
    [Export] public ValueRangeComp<float> ARangeComp { get; private set; }
    #endregion
    #region 重写
    public override void SetValue(Color value)
    {
        var v = new Color(
            Math.Clamp(value.R, RRangeComp.MinValue, RRangeComp.MaxValue),
            Math.Clamp(value.G, GRangeComp.MinValue, GRangeComp.MaxValue),
            Math.Clamp(value.B, BRangeComp.MinValue, BRangeComp.MaxValue),
            Math.Clamp(value.A, ARangeComp.MinValue, ARangeComp.MaxValue)
        );
        WidgetComp.Value = v;
        WidgetComp.DisplayValue = v.ToHtml();
    }
    
    public override void SetCanUse(bool canUse) => WidgetComp.CanUse = canUse;

    public override void SetVisibility(bool visibility) => WidgetComp.Visibility = visibility;
    #endregion
}