using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class WidgetSys<T> : Node
{
    #region 组件
    [Export] public WidgetComp<T> WidgetComp;
    #endregion
    #region 操作
    public virtual void SetValue(T value)
    {
        WidgetComp.Value = value;
        WidgetComp.DisplayValue = value.ToString();
    }

    public virtual void SetCanUse(bool canUse)
    {
        WidgetComp.CanUse = canUse;
    }
    
    public virtual void SetVisibility(bool visibility)
    {
        WidgetComp.Visibility = visibility;
    }
    #endregion
}