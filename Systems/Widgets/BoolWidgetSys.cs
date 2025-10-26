using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class BoolWidgetSys : WidgetSys<bool>
{
    #region 组件
    [Export] public BooleanWidgetComp WidgetComp { get; private set; }
    #endregion
    #region 重写
    public override void SetValue(bool value)
    {
        WidgetComp.Value = value;
        WidgetComp.DisplayValue = value ? "[Value-Bool-True]" : "[Value-Bool-False]";
    }
    
    public override void SetCanUse(bool canUse) => WidgetComp.CanUse = canUse;

    public override void SetVisibility(bool visibility) => WidgetComp.Visibility = visibility;
    #endregion
}