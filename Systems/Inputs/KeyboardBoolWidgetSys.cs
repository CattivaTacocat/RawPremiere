using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems.Inputs;

public partial class KeyboardBoolWidgetSys : Node
{
    #region 组件
    [Export] public WidgetComp<bool> WidgetComp;
    [Export] public HoverComp HoverComp;
    #endregion
    #region 操作
    public override void _Input(InputEvent @event)
    {
        if (!HoverComp.IsHovered) return;
        if (!@event.IsActionPressed("ui_accept")) return;
        WidgetComp.Value = !WidgetComp.Value;
        WidgetComp.DisplayValue = WidgetComp.Value ? "[Value-Bool-True]" : "[Value-Bool-False]";
    }
    #endregion
}