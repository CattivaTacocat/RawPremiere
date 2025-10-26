using System;
using Godot;
using RawPremiere.Components;
using RawPremiere.Components.Enums;

namespace RawPremiere.Systems.Inputs;

public partial class MouseIntegerChangeSys : Node
{
    #region 组件
    [Export] public WidgetComp<int> WidgetComp;
    [Export] public MouseIntegerChangeComp MouseValueChangeComp;
    [Export] public HoverComp HoverComp;
    #endregion
    #region 系统
    [Export] public IntegerWidgetSys IntegerWidgetSys;
    #endregion
    #region 辅助字段
    private bool _isDragging;
    private Vector2 _startPos;
    private Vector2 _offset;
    private int _currentValue;
    #endregion
    #region 操作
    //HACK:可能需要其他的辅助类进行封装
    public override void _Input(InputEvent @event)
    {
        if (!HoverComp.IsHovered) return;
        if (@event is not InputEventMouse em) return;
        switch (em)
        {
            case InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } emb:
                _isDragging = true;
                _currentValue = WidgetComp.Value;
                _startPos = emb.GlobalPosition;
                break;
            case InputEventMouseMotion emm:
                if (_isDragging)
                {
                    _offset = emm.GlobalPosition - _startPos;
                    ShouldWhatToChange(@event);
                }
                break;
            default:
                _isDragging = false;
                _offset = Vector2.Zero;
                break;
        }
    }

    public void CtrlChangeValue(float offset)
    {
        var value = _currentValue + (int)(offset * MouseValueChangeComp.CtrlIncrement);
        SetValue(value);
    }

    public void ShiftChangeValue(float offset)
    {
        var value = _currentValue + (int)(offset * MouseValueChangeComp.ShiftIncrement);
        SetValue(value);
    }

    public void AltChangeValue(float offset)
    {
        var v = (int)offset * MouseValueChangeComp.AltRatio / MouseValueChangeComp.AltRatio;
        var value = _currentValue + v;
        SetValue(value);
    }

    public void NormalChangeValue(float offset)
    {
        var value = _currentValue + (int)(offset * MouseValueChangeComp.NormalIncrement);
        SetValue(value);
    }
    #endregion
    #region 决策
    //HACK:可能需要其他的辅助类进行封装
    private void ShouldWhatToChange(InputEvent @event)
    {
        float offset = 0;
        switch (MouseValueChangeComp.MouseChangeMethod)
        {
            case MouseChangeMethodEnum.Horizontal:
                offset = _offset.X / 100;
                break;
            case MouseChangeMethodEnum.Vertical:
                offset = _offset.Y / -100;
                break;
            case MouseChangeMethodEnum.Roller:
                if (@event is InputEventMouseButton emb)
                    offset = emb.ButtonIndex switch
                    {
                        MouseButton.WheelUp => 1,
                        MouseButton.WheelDown => -1,
                        _ => offset
                    };
                break;
        }

        if (@event is not InputEventKey key)
        {
            NormalChangeValue(offset);
            return;
        }
        switch (key.Keycode)
        {
            case Key.Ctrl:
                CtrlChangeValue(offset);
                break;
            case Key.Shift:
                ShiftChangeValue(offset);
                break;
            case Key.Alt:
                AltChangeValue(offset);
                break;
            default:
                NormalChangeValue(offset);
                break;
        }
    }
    #endregion
    #region 处理
    private void SetValue(int value)
    {
        if (IsInstanceValid(IntegerWidgetSys))
            IntegerWidgetSys.SetValue(value);
        else
            WidgetComp.Value = value;
    }
    #endregion
}