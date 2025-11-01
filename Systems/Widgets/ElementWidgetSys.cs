using Godot;
using RawPremiere.Components;
using RawPremiere.Components.Elements;
using RawPremiere.Objects.Elements;

namespace RawPremiere.Systems;

public partial class ElementWidgetSys : WidgetSys<SimpleElement>
{
    #region 组件
    [Export] public ElementWidgetComp WidgetComp { get; private set; }
    #endregion
    #region 重写
    public override void SetValue(SimpleElement value)
    {
        WidgetComp.Value = value;
        WidgetComp.DisplayValue = value.ElementInfoComp.ElementName;
    }
    
    public override void SetCanUse(bool canUse) => WidgetComp.CanUse = canUse;

    public override void SetVisibility(bool visibility) => WidgetComp.Visibility = visibility;
    #endregion
}