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

    private bool _isRolling;
    private float _rollerOffset;
    #endregion
    #region 操作
    //HACK:可能需要其他的辅助类进行封装
    //FIXME:无法根据不同按键提供不同增量，必须要封装到一些辅助类了
    public override void _Input(InputEvent @event)
    {
        if (!HoverComp.IsHovered) return;
        ShouldRollerToChange(@event);
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
                    ShouldMotionToChange(@event);
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
    private void ShouldMotionToChange(InputEvent @event)
    {
        if (MouseValueChangeComp.MouseChangeMethod == MouseChangeMethodEnum.Roller) return;
        float offset = 0;
        switch (MouseValueChangeComp.MouseChangeMethod)
        {
            case MouseChangeMethodEnum.Horizontal:
                    offset = _offset.X / 100;
                break;
            case MouseChangeMethodEnum.Vertical:
                    offset = _offset.Y / -100;
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

    //HACK:可能需要其他的辅助类进行封装
    private void ShouldRollerToChange(InputEvent @event)
    {
        if (MouseValueChangeComp.MouseChangeMethod != MouseChangeMethodEnum.Roller) return;
        if (@event is InputEventMouseButton em)
        {
            if (em.Pressed && em.ButtonIndex == MouseButton.Middle)
            {
                _isRolling = true;
                _currentValue = WidgetComp.Value;
                _rollerOffset = 0;
            }
            if (!em.Pressed && em.ButtonIndex == MouseButton.Middle)
            {
                _isRolling = false;
            }
            if (!_isRolling) return;
            switch (em.ButtonIndex)
            {
                case MouseButton.WheelUp:
                    _rollerOffset++;
                    break;
                case MouseButton.WheelDown:
                    _rollerOffset--;
                    break;
            }
        }

        var offset = _rollerOffset;
        
        if (!_isRolling) return; 
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