using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class WidgetSys<T> : Node
{
    #region 操作
    public virtual void SetValue(T value) => GD.PrintErr("无实现");

    public virtual void SetCanUse(bool canUse) => GD.PrintErr("无实现");
    
    public virtual void SetVisibility(bool visibility) => GD.PrintErr("无实现");
    #endregion
}