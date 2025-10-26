using System;
using DeadDog.Ordexp;
using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems.Inputs;

public partial class KeyboardEnumWidgetSys<E> : Node where E : Enum
{
    #region 组件
    [Export] public WidgetComp<E> WidgetComp;
    [Export] public HoverComp HoverComp;
    [Export] public EnumComp<E> EnumComp;
    #endregion
    #region 操作
    public override void _Input(InputEvent @event)
    {
        if (!HoverComp.IsHovered) return;
        if (@event.IsActionPressed("ui_left")
            || @event.IsActionPressed("ui_down"))
            SelectPrev();
        else if (@event.IsActionPressed("ui_right")
            || @event.IsActionPressed("ui_up"))
            SelectNext();
    }

    public void SelectPrev()
    {
        var length = EnumComp.Enums.Length;
        EnumComp.CurrentIndex = EnumComp.CurrentIndex.LoopAdd(length, -1);
        WidgetComp.Value = EnumComp.Enums[EnumComp.CurrentIndex];
        WidgetComp.DisplayValue = EnumComp.Enums[EnumComp.CurrentIndex].ToString();
    }
    
    public void SelectNext()
    {
        var length = EnumComp.Enums.Length;
        EnumComp.CurrentIndex = EnumComp.CurrentIndex.LoopAdd(length, 1);
        WidgetComp.Value = EnumComp.Enums[EnumComp.CurrentIndex];
        WidgetComp.DisplayValue = EnumComp.Enums[EnumComp.CurrentIndex].ToString();
    }
    #endregion
}